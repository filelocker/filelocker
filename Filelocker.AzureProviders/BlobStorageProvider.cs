using System;
using System.IO;
using System.Threading.Tasks;
using Filelocker.Domain.Interfaces;

namespace Filelocker.AzureProviders
{
    //TODO: Hopefully Azure Blob Storage libs will be supported by .NET Core 1.1 - Due FALL 2016
    // http://www.hanselman.com/blog/SharingAuthorizationCookiesBetweenASPNET4xAndASPNETCore10.aspx
    public class BlobStorageProvider : IFileStorageProvider
    {
        public BlobStorageProvider(string connectionString)
        {
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
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
            throw new NotImplementedException();
        }
    }
}
