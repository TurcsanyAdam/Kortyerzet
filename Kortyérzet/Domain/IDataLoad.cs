using Kortyérzet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Domain
{
    public interface IDataLoad
    {
        public List<BeerModel> GetBeerList(string queryString);
        public bool CheckIfUserExists(string email, string password);
        void InsertUser(string username, string email, string password);
    }
}
