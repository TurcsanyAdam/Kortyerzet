using Kortyérzet.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Services
{
    public class SqlUserService : SqlBaseService, IUsersService
    { 
        private static User ToUser(IDataReader reader)
        {
            return new User
            (
               (int)reader["user_id"],
               (string)reader["user_name"],
               (string)reader["user_password"],
               (string)reader["user_email"],
               (string)reader["user_role"]

            );
        }

        private readonly IDbConnection _connection;

        public SqlUserService(IDbConnection connection)
        {
            _connection = connection;
        }

        public User GetOne(int userid)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM users WHERE user_id = @userid";

            var param = command.CreateParameter();
            param.ParameterName = "userid";
            param.Value = userid;
            command.Parameters.Add(param);
            using var reader = command.ExecuteReader();
            reader.Read();
            return ToUser(reader);
        }
        public User GetOne(string email)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM users WHERE email = '{email}'";

            using var reader = command.ExecuteReader();
            reader.Read();
            return ToUser(reader);
        }

        public List<User> GetAll()
        {
            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM users";

            using var reader = command.ExecuteReader();
            List<User> users = new List<User>();
            while (reader.Read())
            {
                users.Add(ToUser(reader));
            }
            return users;
        }

        public void DeleteUser(int id)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = "Delete * From users Where userid = @userid";

            var param = command.CreateParameter();
            param.ParameterName = "userid";
            param.Value = id;
            command.Parameters.Add(param);
            using var reader = command.ExecuteReader();
        }

        public User Login(string email, string password)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = $"SELECT * FROM users WHERE user_email = '{email}' AND user_password = '{Utility.Hash(password)}'";


            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return ToUser(reader);
            }
            return null;
        }

        public void Register(string userName, string password, string email, string role)
        {
            using var command = _connection.CreateCommand();

            var userNameParam = command.CreateParameter();
            userNameParam.ParameterName = "username";
            userNameParam.Value = userName;
            var passwordParam = command.CreateParameter();
            passwordParam.ParameterName = "password";
            passwordParam.Value = password;
            var emailParam = command.CreateParameter();
            emailParam.ParameterName = "email";
            emailParam.Value = email;
            var roleParam = command.CreateParameter();
            roleParam.ParameterName = "role";
            roleParam.Value = role;

            command.CommandText = $"INSERT INTO users(username,user_password,email,user_role) VALUES (@username, @password, @email, @role)";
            command.Parameters.Add(userNameParam);
            command.Parameters.Add(passwordParam);
            command.Parameters.Add(emailParam);
            command.Parameters.Add(roleParam);

            HandleExecuteNonQuery(command);

        }

        public bool CheckIfUserExists(string email, string password)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = ($"select true from users where user_email = '{email}' and user_password = '{Utility.Hash(password)}'");
            var param = command.CreateParameter();

            bool UserExist = Convert.ToBoolean(command.ExecuteScalar());

            return UserExist;
        }
    }
}
