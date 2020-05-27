using Kortyérzet.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Services
{
    public class SqlCheckinService: SqlBaseService,ICheckinService
    {
        private readonly IDbConnection _connection;
        private static Checkin ToCheckIn(IDataReader reader)
        {
            return new Checkin(
               (int)reader["user_ID"],
               (int)reader["beer_ID"],
               (string)reader["checkin_comment"],
               Convert.ToInt64(reader["checkin_rating"]),
               (string)reader["checkin_img"]);

        }
        public SqlCheckinService(IDbConnection connection)
        {
            _connection = connection;
        }
        public List<Checkin> GetAllByUserID(int userID)
        {
            List<Checkin> checkinList = new List<Checkin>();
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM checkin WHERE user_ID = {userID}";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                checkinList.Add(ToCheckIn(reader));
            }
            reader.Close();

            return checkinList;
        }
        public List<Checkin> GetAllByBeerID(int beerID)
        {
            List<Checkin> checkinList = new List<Checkin>();
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM checkin WHERE beer_ID = {beerID}";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                checkinList.Add(ToCheckIn(reader));
            }
            reader.Close();

            return checkinList;
        }

        public void InsertCheckin(int userID, int beerID, string comment, float rating, string img)
        {
            using var command = _connection.CreateCommand();

            var userIDParam = command.CreateParameter();
            userIDParam.ParameterName = "user_id";
            userIDParam.Value = userID;
            var beerIDParam = command.CreateParameter();
            beerIDParam.ParameterName = "beer_ID";
            beerIDParam.Value = beerID;
            var commentParam = command.CreateParameter();
            commentParam.ParameterName = "checkin_comment";
            commentParam.Value = comment;
            var ratingParam = command.CreateParameter();
            ratingParam.ParameterName = "checkin_rating";
            ratingParam.Value = rating;
            var imgParam = command.CreateParameter();
            imgParam.ParameterName = "checkin_img";
            imgParam.Value = img;

            command.CommandText = $"INSERT INTO checkin(user_id,beer_ID,checkin_comment,checkin_rating,checkin_img) VALUES (@user_id ,@beer_ID, @checkin_comment, @checkin_rating, @checkin_img)";
            command.Parameters.Add(userIDParam);
            command.Parameters.Add(beerIDParam);
            command.Parameters.Add(commentParam);
            command.Parameters.Add(ratingParam);
            command.Parameters.Add(imgParam);
            HandleExecuteNonQuery(command);

        }
    }
}
