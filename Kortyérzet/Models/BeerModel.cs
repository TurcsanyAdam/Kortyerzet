using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Models
{
    public class BeerModel
    {
        public int ID { get;set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Style { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public int TimesRated { get; set; }
        public int CheckIn { get; set; }
        public int ABV { get; set; }
        public int IBU { get; set; }
        public string Brewery { get; set; }

        public BeerModel(int id, string name, string logo, string style, string description, int rating, int timesRated, int checkIn, int abv, int ibu, string brewery)
        {
            ID = id;
            Name = name;
            Logo = logo;
            Style = style;
            Description = description;
            Rating = rating;
            TimesRated = timesRated;
            CheckIn = checkIn;
            ABV = abv;
            IBU = ibu;
            Brewery = brewery;
        }

    }
}
