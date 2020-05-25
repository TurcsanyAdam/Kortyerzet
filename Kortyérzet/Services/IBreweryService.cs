using Kortyérzet.Domain;
using Kortyérzet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Services
{
    public interface IBreweryService
    {
        List<Brewery> GetAll();
        Brewery GetOne(int id);
        Brewery GetOne(string breweryName);
    }
}
