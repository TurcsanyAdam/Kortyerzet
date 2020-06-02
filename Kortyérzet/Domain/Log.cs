using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Domain
{
    public class Log
    {
        public int UserID { get; set; }
        public string UserActivity { get; set; }
        public DateTime SubmissionTime { get; set; }
        
        public Log(int userId, string activity, DateTime submisison)
        {
            UserID = userId;
            UserActivity = activity;
            SubmissionTime = submisison;
        }
    }
}
