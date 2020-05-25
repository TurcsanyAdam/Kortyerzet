using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Domain
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }


        public User(int id, string username, string password, string email, string role)
        {
            ID = id;
            Username = username;
            Password = password;
            Email = email;
            Role = role;
        }
        public User()
        {

        }
    }
}
