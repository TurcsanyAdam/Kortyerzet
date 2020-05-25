using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Services
{
    public interface IStorageService
    {

        string Save(string fileName, Stream stream);
        void Delete(string fileName);

    }
}
