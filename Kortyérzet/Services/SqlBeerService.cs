using Kortyérzet.Domain;
using Kortyérzet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Services
{
    public class SqlBeerService : IBeerService
    {
        public static List<string> AllStyles = new List<string>();
        private readonly IDbConnection _connection;
        private static Beer ToBeer(IDataReader reader)
        {
            return new Beer(
               (int)reader["beer_ID"],
               (string)reader["beer_name"],
               (string)reader["beer_logo"],
               (string)reader["beer_style"],
               (string)reader["beer_desc"],
               (int)reader["beer_rating"],
               (int)reader["beer_timesRated"],
               (int)reader["beer_checkin"],
               Convert.ToInt64(reader["beer_abv"]),
               (int)reader["beer_ibu"],
               (int)reader["brewery_ID"]);




        }
        public SqlBeerService(IDbConnection connection)
        {
            _connection = connection;
        }
        public List<Beer> GetAll()
        {
            List<Beer> breweryList = new List<Beer>();
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM beer";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                breweryList.Add(ToBeer(reader));
            }
            reader.Close();
            GetAllStyle();

            return breweryList;
        }
        public List<Beer> GetAllByStyle(string beerStyle)
        {
            List<Beer> breweryList = new List<Beer>();
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT* FROM beer WHERE beer_style = @beerStyle";

            var param = command.CreateParameter();
            param.ParameterName = "beerStyle";
            param.Value = beerStyle;
            command.Parameters.Add(param);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                breweryList.Add(ToBeer(reader));
            }
            return breweryList;
        }
        public List<Beer> GetAllByBrewery(int breweryID)
        {
            List<Beer> breweryList = new List<Beer>();
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM brewery JOIN beer on brewery.brewery_id = beer.brewery_id WHERE brewery.brewery_id = @breweryID";

            var param = command.CreateParameter();
            param.ParameterName = "breweryID";
            param.Value = breweryID;
            command.Parameters.Add(param);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                breweryList.Add(ToBeer(reader));
            }
            return breweryList;
        }
        public void GetAllStyle()
        {
            AllStyles.Clear();

            using var command = _connection.CreateCommand();
            command.CommandText = "select distinct beer_style from beer";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                AllStyles.Add(Convert.ToString(reader[0]));
            }
        }

        public Beer GetOne(int id)
        {

            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM beer WHERE beer_id = @id";
            var param = command.CreateParameter();
            param.ParameterName = "id";
            param.Value = id;
            command.Parameters.Add(param);
            using var reader = command.ExecuteReader();

            reader.Read();
            return ToBeer(reader);

        }

        public Beer GetOne(string beerName)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM brewery WHERE beer_name = @beerName";
            var param = command.CreateParameter();
            param.ParameterName = "beerName";
            param.Value = beerName;
            command.Parameters.Add(param);
            using var reader = command.ExecuteReader();

            reader.Read();
            return ToBeer(reader);
        }
    }
}
