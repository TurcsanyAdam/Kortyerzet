using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Kortyérzet.Domain;
using Kortyérzet.Models;
using Kortyérzet.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kortyérzet.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ICheckinService _checkinService;
        private readonly IBeerService _beerService;
        public ProfileController(ICheckinService checkinService, IBeerService beerService)
        {
            _checkinService = checkinService;
            _beerService = beerService;

        }

        public IActionResult ProfileDetails()
        {
            var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("ID"));
            List<CheckinModel> checkinModels = new List<CheckinModel>();
            List<Checkin> checkins = _checkinService.GetAllByUserID(userId);

            foreach(Checkin checkin in checkins)
            {
                Beer beer = _beerService.GetOne(checkin.BeerID);
                CheckinModel checkinModel = new CheckinModel(checkin, beer);
                checkinModels.Add(checkinModel);
            }
            return View(checkinModels);
        }
    }
}
