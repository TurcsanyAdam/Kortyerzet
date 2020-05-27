using Kortyérzet.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Services
{
    public interface ICheckinService
    {
        List<Checkin> GetAllByUserID(int userID);
        List<Checkin> GetAllByBeerID(int beerID);
        void InsertCheckin (int userID, int beerID, string comment, float rating, string img);
    }
}
