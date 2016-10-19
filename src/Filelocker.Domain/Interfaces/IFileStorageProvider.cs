using System;
using System.IO;
using System.Threading.Tasks;

namespace Filelocker.Domain.Interfaces
{
    public interface IFileStorageProvider
    {
        Stream GetWriteStream(string fileName);

        Stream GetReadStream(string fileName);

        Task DeleteFile(string fileName);
    }
}
