using Kortyérzet.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Services
{
    public interface ILoggerService
    {
        void LogActivity(int userID,string activity, DateTime dateTime);
        List<Log> GetAll();
    }
}
