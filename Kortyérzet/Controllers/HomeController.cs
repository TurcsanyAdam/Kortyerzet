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

namespace Kortyérzet.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUsersService _userService;
        private readonly IBeerService _beerService;
        private readonly IBreweryService _breweryService;
        private readonly IStorageService _storageService;

        public HomeController(IUsersService userService, IBeerService beerService, IBreweryService breweryService, IStorageService storageService)
        {
            _userService = userService;
            _beerService = beerService;
            _breweryService = breweryService;
            _storageService = storageService;
        }

        public IActionResult Index()
        {
            List<Beer> beerList = _beerService.GetAll();
            List<BeerModel> beerModeList = new List<BeerModel>();
            foreach (Beer beer in beerList)
            {
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public IActionResult BeerDetails(int id)
        {
            Beer beer = _beerService.GetOne(id);
            Brewery brewery = _breweryService.GetOne(beer.BreweryID);
            BeerModel beerModel = new BeerModel(beer, brewery);


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

                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files[0];
                    if (file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);

                        using Stream imageStream = file.OpenReadStream();
                        _storageService.Save(fileName, imageStream);
                        checkin.Img = fileName;

                    }

                    return RedirectToAction("Index");
                }
            }

            return View();
        }
        public IActionResult Style()
        {
            string chosenSytle = Request.Form["chosenStyle"];

            List<Beer> beerList = _beerService.GetAllByStyle(chosenSytle);
            List<BeerModel> beerModeList = new List<BeerModel>();
            foreach(Beer beer in beerList)
            {
                Brewery brewery = _breweryService.GetOne(beer.BreweryID);
                BeerModel beerModel = new BeerModel(beer, brewery);
                beerModeList.Add(beerModel);

            }
            return Json(beerModeList);
        }
        public IActionResult Recommendation()
        {
            string style = Convert.ToString(Request.Form["chosenStyle"]);
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
    }
}
