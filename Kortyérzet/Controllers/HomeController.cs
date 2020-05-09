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

namespace Kortyérzet.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataLoad _loader;

        public HomeController(IDataLoad loader)
        {
            _loader = loader;
        }

        public IActionResult Index()
        {           
            return View(ListBeersByRating());
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

        private List<BeerModel> ListBeersByRating()
        {
            return _loader.GetBeerList("SELECT * FROM beer ORDER BY beer_rating DESC;");
        }
        [HttpGet]
        public IActionResult BeerDetails(int id)
        {
            BeerModel beer = _loader.GetBeerList($"SELECT * FROM beer WHERE beer_id = {id};")[0];


            return View(beer);
        }
        public IActionResult Style()
        {
            string temp = Request.Form["chosenStyle"];
            List<BeerModel> beerList = _loader.GetBeerList($"SELECT * FROM beer WHERE beer_style = '{temp}';");
            return Json(beerList);
        }
    }
}
