using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Kortyérzet.Models;
using Kortyérzet.Domain;
using System.Text.Json;
using Newtonsoft.Json;
using Kortyérzet.Services;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace Kortyérzet.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUsersService _userService;
        private readonly IBeerService _beerService;
        private readonly IBreweryService _breweryService;
        private readonly IStorageService _storageService;
        private readonly ICheckinService _checkinService;
        private readonly ILoggerService _loggerService;

        public HomeController(IUsersService userService, IBeerService beerService, IBreweryService breweryService, IStorageService storageService, ICheckinService checkinService, ILoggerService loggerService)
        {
            _userService = userService;
            _beerService = beerService;
            _breweryService = breweryService;
            _storageService = storageService;
            _checkinService = checkinService;
            _loggerService = loggerService;
        }

        public IActionResult Index()
        {

            List<Beer> beerList = _beerService.GetAll();
            List<BeerModel> beerModeList = new List<BeerModel>();
            foreach (Beer beer in beerList)
            {
                float[] ratings = _beerService.GetRating(beer.ID);
                beer.Rating = ratings[1] / ratings[0];
                beer.TimesRated = Convert.ToInt32(ratings[0]);
                if (ratings[1] != 0)
                {
                    _beerService.UpdateRating(ratings);

                }
                Brewery brewery = _breweryService.GetOne(beer.BreweryID);
                BeerModel beerModel = new BeerModel(beer, brewery);
                beerModeList.Add(beerModel);

            }
            return View(beerModeList);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Log()
        {

            return View(_loggerService.GetAll());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult BeerDetails(int id)
        {

            Beer beer = _beerService.GetOne(id);
            Brewery brewery = _breweryService.GetOne(beer.BreweryID);
            List<Checkin> checkins = _checkinService.GetAllByBeerID(id);


            float[] ratings = _beerService.GetRating(beer.ID);
            beer.Rating = ratings[1] / ratings[0];
            beer.TimesRated = Convert.ToInt32(ratings[0]);
            if (ratings[1] != 0)
            {
                _beerService.UpdateRating(ratings);

            }
            foreach (Checkin checkin in checkins)
            {
                checkin.User = _userService.GetOne(checkin.UserID);
            }
            BeerModel beerModel = new BeerModel(beer, brewery);
            beerModel.Checkins = checkins;

            return View(beerModel);
        }
        [HttpPost]
        public IActionResult BeerDetails(BeerModel model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult Upload()
        {

            if (ModelState.IsValid)
            {
                var newCheckInData = Request.Form.ToList();
                var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("ID"));
                var beerID = Convert.ToInt32(newCheckInData[0].Value);
                string newCheckInText = Convert.ToString(newCheckInData[1].Value);
                string newCheckInRatingString = Convert.ToString(newCheckInData[2].Value);
                float newCheckInRating = float.Parse(newCheckInRatingString.Trim(), CultureInfo.InvariantCulture.NumberFormat);
                Checkin checkin = new Checkin(userId, beerID, newCheckInText, newCheckInRating);
                string fileName = null;

                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files[0];
                    if (file.Length > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);

                        using Stream imageStream = file.OpenReadStream();
                        _storageService.Save(fileName, imageStream);
                        checkin.Img = fileName;

                    }

                }

                _checkinService.InsertCheckin(userId,beerID,newCheckInText,newCheckInRating,fileName);
                _beerService.UpdateRating(newCheckInRating, beerID);
                _loggerService.LogActivity(userId, $"Check in with beer ID:{beerID}", DateTime.Now);
                return RedirectToAction("Index");

            }

            return View();
        }
        public IActionResult Style()
        {
            string chosenSytle = Request.Form["chosenStyle"];
            List<Beer> beerList = new List<Beer>();
            if (chosenSytle.StartsWith("Select"))
            {
                beerList = _beerService.GetAll();
            }
            else
            {
                beerList = _beerService.GetAllByStyle(chosenSytle);

            }

            List<BeerModel> beerModelList = new List<BeerModel>();
            foreach(Beer beer in beerList)
            {
                Brewery brewery = _breweryService.GetOne(beer.BreweryID);
                BeerModel beerModel = new BeerModel(beer, brewery);
                beerModelList.Add(beerModel);

            }
            return Json(beerModelList);
        }
        public IActionResult Recommendation()
        {
            string style = Request.Form["chosenStyle"];
            int breweryID = Convert.ToInt32(Request.Form["breweryID"]);
            int beerID = Convert.ToInt32(Request.Form["beerID"]);

            List<Beer> beerListBrewery = _beerService.GetAllByBrewery(breweryID);
            List<Beer> beerListStyle = _beerService.GetAllByStyle(style);


            List<BeerModel> beerModelList = new List<BeerModel>();

            foreach (Beer beer in beerListBrewery)
            {
                if(beerModelList.Count < 5)
                {
                    Brewery brewery = _breweryService.GetOne(beer.BreweryID);
                    BeerModel beerModel = new BeerModel(beer, brewery);
                    if (!beerModelList.Contains(beerModel) && beer.ID != beerID)
                    {
                        beerModelList.Add(beerModel);
                    }

                }

            }
            foreach (Beer beer in beerListStyle)
            {
                if (beerModelList.Count < 5)
                {
                    Brewery brewery = _breweryService.GetOne(beer.BreweryID);
                    BeerModel beerModel = new BeerModel(beer, brewery);
                    if (!beerModelList.Contains(beerModel) && beer.ID != beerID)
                    {
                        beerModelList.Add(beerModel);
                    }
                }

            }


            return Json(beerModelList);
        }

        public IActionResult NewBeerOrBrewery()
        {
            List<Brewery> breweries = _breweryService.GetAll();
            return View(breweries);

        }
        public IActionResult NewBeer()
        {
            if (ModelState.IsValid)
            {
                var newBeerData = Request.Form.ToList();
                var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("ID"));
                string newBeerName = Convert.ToString(newBeerData[0].Value);
                string fileName = null;
                string newBeerStyle = Convert.ToString(newBeerData[1].Value);
                string newBeerDesc = Convert.ToString(newBeerData[3].Value);
                float newBeerBreweryABV = float.Parse(newBeerData[4].Value, CultureInfo.InvariantCulture.NumberFormat);
                int newBeerBreweryIBU = Convert.ToInt32(newBeerData[5].Value);
                int newBeerBreweryID = _breweryService.GetOne(Convert.ToString(newBeerData[2].Value)).ID;



                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files[0];
                    if (file.Length > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);

                        using Stream imageStream = file.OpenReadStream();
                        _storageService.Save(fileName, imageStream);

                    }

                }
                _beerService.InsertBeer(newBeerName, fileName, newBeerStyle, newBeerDesc, newBeerBreweryABV, newBeerBreweryIBU, newBeerBreweryID);
                _loggerService.LogActivity(userId, $"Add new beer with beer name:{newBeerName}", DateTime.Now);

                return RedirectToAction("Index");

            }

            return View();
        }

        public IActionResult NewBrewery()
        {
            if (ModelState.IsValid)
            {
                var newBreweryData = Request.Form.ToList();
                var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("ID"));
                string newBreweryName = Convert.ToString(newBreweryData[0].Value);
                string fileName = null;
                string newBreweryHQ = Convert.ToString(newBreweryData[1].Value);
                string newBreweryDesc = Convert.ToString(newBreweryData[2].Value);




                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files[0];
                    if (file.Length > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);

                        using Stream imageStream = file.OpenReadStream();
                        _storageService.Save(fileName, imageStream);

                    }

                }
                _breweryService.InsertBeer(newBreweryName, fileName, newBreweryHQ, newBreweryDesc);
                _loggerService.LogActivity(userId, $"Add new beer with brewery name:{newBreweryName}", DateTime.Now);

                return RedirectToAction("Index");

            }

            return View();
        }
    }
}
