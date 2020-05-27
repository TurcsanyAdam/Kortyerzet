using Kortyérzet.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Kortyérzet.Models
{
    public class BeerModel
    {
        public int ID { get;set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Style { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public int TimesRated { get; set; }
        public int CheckIn { get; set; }
        public float ABV { get; set; }
        public int IBU { get; set; }
        public Brewery Brewery { get; set; }
        public List<Checkin> Checkins { get; set; }

        public BeerModel(Beer beer, Brewery brewery)
        {
            ID = beer.ID;
            Name = beer.Name;
            Logo = beer.Logo;
            Style = beer.Style;
            Description = beer.Description;
            Rating = beer.Rating;
            TimesRated = beer.TimesRated;
            CheckIn = beer.CheckIn;
            ABV = beer.ABV;
            IBU = beer.IBU;
            Brewery = brewery;
        }

        public BeerModel()
        {

        }

    }
}
