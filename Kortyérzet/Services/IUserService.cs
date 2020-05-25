using Kortyérzet.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Services
{
    public interface IUsersService
    {
        User GetOne(int userid);
        User GetOne(string email);
        List<User> GetAll();
        void DeleteUser(int id);
        bool CheckIfUserExists(string email, string password);
        User Login(string email, string password);
        void Register(string username, string password, string email, string role);
    }
}
