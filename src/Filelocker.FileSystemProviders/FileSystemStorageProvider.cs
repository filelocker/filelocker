using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Filelocker.Domain.Interfaces;

namespace Filelocker.FileSystemProviders
{
    public class FileSystemStorageProvider : IFileStorageProvider
    {
        private readonly string _basePath;

        public FileSystemStorageProvider(string basePath)
        {
            _basePath = basePath;
        }

        public Task SaveAsync(Stream fileStream, string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> ReadAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public Stream GetStream(string fileName)
        {
            var filePath = Path.Combine(_basePath, fileName);
            var fileStream = new FileStream(filePath, FileMode.Append);
            return fileStream;
        }
    }
}
