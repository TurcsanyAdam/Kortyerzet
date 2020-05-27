using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Domain
{
    public class Checkin
    {
        public int UserID { get; set; }
        public User User{ get; set; }
        public int BeerID { get; set; }
        public string Comment { get; set; }
        public float Rating { get; set; }

        public string Img { get; set; }
        public Checkin(int userID, int beerID, string comment, float rating)
        {
            UserID = userID;
            BeerID = beerID;
            Comment = comment;
            Rating = rating;
        }
        public Checkin(int userID, int beerID, string comment, float rating, string img)
        {
            UserID = userID;
            BeerID = beerID;
            Comment = comment;
            Rating = rating;
            Img = img;
        }
    }

}
