using Kortyérzet.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Domain
{
    public class DataBaseLoader:IDataLoad
    {
        private static readonly string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        private static readonly string dbUser = Environment.GetEnvironmentVariable("DB_USER");
        private static readonly string dbPass = Environment.GetEnvironmentVariable("DB_PASS");
        private static readonly string dbName = Environment.GetEnvironmentVariable("DB_NAME");
        public static readonly string connectingString = $"Host={dbHost};Username={dbUser};Password={dbPass};Database={dbName}";
        public List<BeerModel> BeerList { get; set; } = new List<BeerModel>();
        public static List<string> BeerStyles { get; set; } = new List<string>();

        public List<BeerModel> GetBeerList(string queryString)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                BeerList.Clear();
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(queryString, connection);
                NpgsqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    BeerModel beer = new BeerModel(id: Convert.ToInt32(dataReader[0]),
                                                       name: dataReader[1].ToString(),
                                                       logo: dataReader[2].ToString(),
                                                       style: dataReader[3].ToString(),
                                                       description: dataReader[4].ToString(),
                                                       rating: Convert.ToInt32(dataReader[5]),
                                                       timesRated: Convert.ToInt32(dataReader[6]),
                                                       checkIn: Convert.ToInt32(dataReader[7]),
                                                       abv: Convert.ToInt32(dataReader[8]),
                                                       ibu: Convert.ToInt32(dataReader[9]),
                                                       brewery: GetBrewery(Convert.ToInt32(dataReader[10])).Name);
                    if (!BeerStyles.Contains(beer.Style))
                    {
                        BeerStyles.Add(beer.Style);

                    }

                    BeerList.Add(beer);
                }
            }

            return BeerList;
        }
        public bool CheckIfUserExists(string email, string password)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand($"select true from users where email = '{email}' and user_password = '{Utility.Hash(password)}'", connection);
                command.Parameters.AddWithValue("email", email);
                command.Parameters.AddWithValue("password", password);
                bool UserExist = false;
                UserExist = Convert.ToBoolean(command.ExecuteScalar());
                return UserExist;

            }
        }
        public void InsertUser(string username, string email, string password)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO users(username, user_password, registration_time, email)" +
                    $"VALUES ((@username), (@password), (@time), (@email),(@checkedInBeers))", connection);
                command.Parameters.AddWithValue("username", username);
                command.Parameters.AddWithValue("password", password);
                command.Parameters.AddWithValue("time", Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                command.Parameters.AddWithValue("email", email);

                command.ExecuteNonQuery();


            }
        }
        public BreweryModel GetBrewery(int id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand($"SELECT * FROM brewery WHERE brewery_id = {id}", connection);
                NpgsqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    BreweryModel brewery = new BreweryModel(id: Convert.ToInt32(dataReader[0]),
                                                       name: dataReader[1].ToString(),
                                                       logo: dataReader[2].ToString(),
                                                       hq: dataReader[3].ToString(),
                                                       description: dataReader[4].ToString(),
                                                       beerCount: Convert.ToInt32(dataReader[5]),
                                                       rating: Convert.ToInt32(dataReader[6]),
                                                       timesRated: Convert.ToInt32(dataReader[7]),
                                                       checkIn: Convert.ToInt32(dataReader[8]));

                    return brewery;

                }

            }
            return null;

        }

    }
}
