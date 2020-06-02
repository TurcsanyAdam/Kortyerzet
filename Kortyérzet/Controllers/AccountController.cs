using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Kortyérzet.Domain;
using Kortyérzet.Services;
using Kortyérzet.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kortyérzet.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersService _userService;
        private readonly ILoggerService _loggerService;


        public AccountController(IUsersService usersService, ILoggerService loggerService )
        {
            _userService = usersService;
            _loggerService = loggerService;

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();

        }
        public IActionResult Registration(RegistrationViewModel model)
        {
            if (!Utility.IsValidEmail(model.Email))
            {
                return RedirectToAction("Registration", "Account");
            }

            _userService.Register(model.Username, model.Password, model.Email, "user");

            User user = _userService.GetOne(model.Email);
            _loggerService.LogActivity(user.ID, "User registeration ", DateTime.Now);



            return RedirectToAction("Index", "Home");

        }
        public IActionResult RegistrationComplete([FromForm]string username, [FromForm]string email, [FromForm] string passwordA)
        {
            if (!Utility.IsValidEmail(email))
            {
                return RedirectToAction("Registration", "Account");

            }
            //_loader.InsertUser(username, email, Utility.Hash(passwordA));
            return View();

        }
        [Authorize]
        [HttpGet]

        public async Task<ActionResult> LogOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]

        public async Task<ActionResult> LoginAsync(LoginViewModel model)
        {
            User user = _userService.Login(model.Email, model.Password);
            
            if (user != null)
            {


                await HttpContext.SignInAsync(
                                    CookieAuthenticationDefaults.AuthenticationScheme,
                                    new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                                    {
                        new Claim("Email", user.Email),
                        new Claim("ID", user.ID.ToString()),
                        new Claim(ClaimTypes.Role, user.Role),
                        new Claim("Username", user.Username),
                                    }, CookieAuthenticationDefaults.AuthenticationScheme)),
                                    new AuthenticationProperties());

                _loggerService.LogActivity(user.ID, $"New login from user {user.Username}", DateTime.Now);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Account");


            }

        }
    }

}
