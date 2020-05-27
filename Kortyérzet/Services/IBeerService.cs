using Kortyérzet.Domain;
using Kortyérzet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Services
{
    public interface IBeerService
    {
        List<Beer> GetAll();
        List<Beer> GetAllByStyle(string style);
        List<Beer> GetAllByBrewery(int breweryID);
        void GetAllStyle();
        void InsertBeer(string beerName, string beerLogo, string beerStyle, string beerDesc, float beerABV, int beerIBU, int breweryID);
        Beer GetOne(int id);
        Beer GetOne(string beerName);
        float[] GetRating (int id);
        void UpdateRating(float[] ratings);

        void UpdateRating(float rating, int id);
    }
}
