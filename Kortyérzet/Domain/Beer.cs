using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Domain
{
    public class Beer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Style { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public int TimesRated { get; set; }
        public int CheckIn { get; set; }
        public float ABV { get; set; }
        public int IBU { get; set; }
        public int BreweryID { get; set; }

        public Beer(int id, string name, string logo, string style, string description, float rating, int timesRated, int checkIn, float abv, int ibu, int breweryID)
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
            BreweryID = breweryID;
        }
    }
}
