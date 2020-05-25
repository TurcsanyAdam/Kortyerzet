using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Domain
{
    public class Brewery
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string HQ { get; set; }
        public string Description { get; set; }
        public int BeerCount { get; set; }
        public int Rating { get; set; }
        public int TimesRated { get; set; }
        public int CheckIn { get; set; }

        public Brewery(int id, string name, string logo, string hq, string description, int beerCount, int rating, int timesRated, int checkIn)
        {
            ID = id;
            Name = name;
            Logo = logo;
            HQ = hq;
            Description = description;
            BeerCount = beerCount;
            Rating = rating;
            TimesRated = timesRated;
            CheckIn = checkIn;

        }
    }
}
