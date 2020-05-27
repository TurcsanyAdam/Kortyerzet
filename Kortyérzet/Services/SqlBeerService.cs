using Kortyérzet.Domain;
using Kortyérzet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Services
{
    public class SqlBeerService : SqlBaseService,IBeerService
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
               Convert.ToInt64(reader["beer_rating"]),
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
            command.CommandText = $"SELECT * FROM beer order BY beer_rating desc";

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

        public float[] GetRating(int id)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = $"select count(*) as rowcount ,sum(checkin_rating) as total from checkin WHERE beer_id = @id";
            var param = command.CreateParameter();
            param.ParameterName = "id";
            param.Value = id;
            command.Parameters.Add(param);
            using var reader = command.ExecuteReader();
            int rowcount = 0;
            float totalRating = 0;
            
            while (reader.Read())
            {
                rowcount = Convert.ToInt32(reader[0]);
                if(rowcount != 0)
                {
                    totalRating = Convert.ToSingle(reader[1]);

                }
            }
            float[] ratings = new float[3] { rowcount, totalRating, id };
            return ratings;
        }
        public void UpdateRating(float[] ratings)
        {
            using var command = _connection.CreateCommand();
            float rating = (ratings[1] / ratings[0]);
            command.CommandText = $"UPDATE beer SET beer_rating = @rating, beer_timesrated = {ratings[0]} WHERE beer_id = {ratings[2]}";

            var param = command.CreateParameter();
            param.ParameterName = "rating";
            param.Value = rating;
            command.Parameters.Add(param);

            using var reader = command.ExecuteReader();

            reader.Read();
        }

        public void UpdateRating(float rating, int id)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = $"UPDATE beer SET beer_rating = @rating, beer_timesrated = beer_timesrated + 1 WHERE beer_id = {id}";

            var param = command.CreateParameter();
            param.ParameterName = "rating";
            param.Value = rating;
            command.Parameters.Add(param);

            using var reader = command.ExecuteReader();

            reader.Read();
        }

        public void InsertBeer(string beerName, string beerLogo, string beerStyle, string beerDesc, float beerABV, int beerIBU, int breweryID)
        {
            using var command = _connection.CreateCommand();

            var beerNameParam = command.CreateParameter();
            beerNameParam.ParameterName = "beer_name";
            beerNameParam.Value = beerName;

            var beerLogoParam = command.CreateParameter();
            beerLogoParam.ParameterName = "beer_logo";
            beerLogoParam.Value = beerLogo;

            var beerStlyeParam = command.CreateParameter();
            beerStlyeParam.ParameterName = "beer_style";
            beerStlyeParam.Value = beerStyle;

            var beerDescParam = command.CreateParameter();
            beerDescParam.ParameterName = "beer_desc";
            beerDescParam.Value = beerDesc;

            var beerABVParam = command.CreateParameter();
            beerABVParam.ParameterName = "beer_abv";
            beerABVParam.Value = beerABV;

            var beerIBUParam = command.CreateParameter();
            beerIBUParam.ParameterName = "beer_ibu";
            beerIBUParam.Value = beerIBU;

            var beerBreweryIDParam = command.CreateParameter();
            beerBreweryIDParam.ParameterName = "brewery_ID";
            beerBreweryIDParam.Value = breweryID;

            command.CommandText = $"insert into beer (beer_name, beer_logo, beer_style, beer_desc, beer_rating, beer_timesRated, beer_checkin, beer_abv, beer_ibu, brewery_ID) values" +
                "(@beer_name, @beer_logo, @beer_style, @beer_desc, 0, 0, 0, @beer_abv, @beer_ibu, @brewery_ID)";

            command.Parameters.Add(beerNameParam);
            command.Parameters.Add(beerLogoParam);
            command.Parameters.Add(beerStlyeParam);
            command.Parameters.Add(beerDescParam);
            command.Parameters.Add(beerABVParam);
            command.Parameters.Add(beerIBUParam);
            command.Parameters.Add(beerBreweryIDParam);

            HandleExecuteNonQuery(command);
        }
    }
}
