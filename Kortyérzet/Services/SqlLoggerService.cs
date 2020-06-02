using Kortyérzet.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Services
{
    public class SqlLoggerService : SqlBaseService, ILoggerService
    {
        private readonly IDbConnection _connection;

        public SqlLoggerService(IDbConnection connection)
        {
            _connection = connection;
        }

        private static Log ToLog(IDataReader reader)
        {
            return new Log(
               (int)reader["user_id"],
               (string)reader["user_activity"],
               (DateTime)reader["submission_time"]);





        }

        public List<Log> GetAll()
        {
            List<Log> logs = new List<Log>();

            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM activity";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                logs.Add(ToLog(reader));
            }
            reader.Close();

            return logs;
        }

        public void LogActivity(int userID, string activity, DateTime dateTime)
        {
            using var command = _connection.CreateCommand();

            var userIDParam = command.CreateParameter();
            userIDParam.ParameterName = "user_id";
            userIDParam.Value = userID;

            var activityParam = command.CreateParameter();
            activityParam.ParameterName = "user_activity";
            activityParam.Value = activity;

            var submissionParam = command.CreateParameter();
            submissionParam.ParameterName = "submission_time";
            submissionParam.Value = dateTime;


            command.CommandText = $"insert into activity (user_id, user_activity, submission_time) values" +
                "(@user_id, @user_activity, @submission_time)";

            command.Parameters.Add(userIDParam);
            command.Parameters.Add(activityParam);
            command.Parameters.Add(submissionParam);
            HandleExecuteNonQuery(command);
        }
    }
}
