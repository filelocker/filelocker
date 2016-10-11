using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Filelocker.Domain.Interfaces
{
    public interface IFileStorageProvider
    {
        Task SaveAsync(Stream fileStream, string fileName);

        Task<Stream> ReadAsync(string fileName);

        Stream GetStream(string fileName);
    }
}
