using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Kortyérzet.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kortyérzet.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        public ProfileController()
        {
        }

        public IActionResult ProfileDetails()
        {
            //var user = HttpContext.User;
            //var claim = user.Claims.First(c => c.Type == ClaimTypes.Email);
            //var email = claim.Value;

            //User searchedUser = _loader.GetUserList($"Select * FROM users WHERE email = '{email}'")[0];
            //List<QuestionModel> searchedQuestionsList = _loader.GetUserQuestions(searchedUser, "SELECT q.* FROM question q " +
            //                                                            "LEFT JOIN answer a ON q.question_id = a.question_id " +
            //                                                            $"WHERE q.userid = {searchedUser.UserId} " +
            //                                                            $"OR a.userid = {searchedUser.UserId} " +
            //                                                            "GROUP BY q.question_id; ");

            //ProfileDetailsModel profileDetailModel = new ProfileDetailsModel(searchedUser, searchedQuestionsList);
            return View();
        }
    }
}
