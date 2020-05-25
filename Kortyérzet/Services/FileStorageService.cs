using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.Services
{
    public class FileStorageService : IStorageService
    {
        private readonly string _uploadsDirectory;

        public FileStorageService(string uploadsDirectory)
        {
            _uploadsDirectory = uploadsDirectory;
        }

        public string Save(string fileName, Stream stream)
        {
            using Stream outputStream = new FileStream(Path.Combine(_uploadsDirectory, fileName), FileMode.Create, FileAccess.Write);
            stream.CopyTo(outputStream);
            return fileName;
        }

        public void Delete(string fileName)
        {
            File.Delete(Path.Combine(_uploadsDirectory, fileName));
        }
    }
}
