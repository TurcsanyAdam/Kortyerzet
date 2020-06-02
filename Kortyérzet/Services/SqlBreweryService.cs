using Kortyérzet.Domain;
using Kortyérzet.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Services
{
    public class SqlBreweryService : SqlBaseService,IBreweryService
    {
        private readonly IDbConnection _connection;
        private static Brewery ToBrewery(IDataReader reader)
        {
            return new Brewery(
               (int)reader["brewery_ID"],
               (string)reader["brewery_name"],
               (string)reader["brewery_logo"],
               (string)reader["brewery_HQ"],
               (string)reader["brewery_desc"],
               (int)reader["brewery_beerCount"],
               (int)reader["brewery_rating"],
               (int)reader["brewery_timesRated"],
               (int)reader["brewery_checkin"]);




        }
        public SqlBreweryService(IDbConnection connection)
        {
            _connection = connection;
        }
        public List<Brewery> GetAll()
        {
            List<Brewery> breweryList = new List<Brewery>();
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM brewery";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                breweryList.Add(ToBrewery(reader));
            }
            return breweryList;
        }

        public Brewery GetOne(int id)
        {

            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM brewery WHERE brewery_id = @id";
            var param = command.CreateParameter();
            param.ParameterName = "id";
            param.Value = id;
            command.Parameters.Add(param);
            using var reader = command.ExecuteReader();

            reader.Read();
            return ToBrewery(reader);

        }

        public Brewery GetOne(string breweryName)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM brewery WHERE brewery_name = @breweryName";
            var param = command.CreateParameter();
            param.ParameterName = "breweryName";
            param.Value = breweryName;
            command.Parameters.Add(param);
            using var reader = command.ExecuteReader();

            reader.Read();
            return ToBrewery(reader);
        }
        public void InsertBeer(string breweryName, string breweryLogo, string breweryHQ, string breweryDesc)
        {
            using var command = _connection.CreateCommand();

            var breweryNameParam = command.CreateParameter();
            breweryNameParam.ParameterName = "brewery_name";
            breweryNameParam.Value = breweryName;

            var breweryLogoParam = command.CreateParameter();
            breweryLogoParam.ParameterName = "brewery_logo";
            breweryLogoParam.Value = breweryLogo;

            var breweryHQParam = command.CreateParameter();
            breweryHQParam.ParameterName = "brewery_HQ";
            breweryHQParam.Value = breweryHQ;

            var breweryDescParam = command.CreateParameter();
            breweryDescParam.ParameterName = "brewery_desc";
            breweryDescParam.Value = breweryDesc;


            command.CommandText = $"insert into brewery (brewery_name, brewery_logo, brewery_HQ, brewery_desc, brewery_beercount, brewery_rating, brewery_timesRated, brewery_checkin) values" +
                "(@brewery_name, @brewery_logo, @brewery_HQ, @brewery_desc,0, 0, 0, 0)";

            command.Parameters.Add(breweryNameParam);
            command.Parameters.Add(breweryLogoParam);
            command.Parameters.Add(breweryHQParam);
            command.Parameters.Add(breweryDescParam);


            HandleExecuteNonQuery(command);
        }
    }
}
