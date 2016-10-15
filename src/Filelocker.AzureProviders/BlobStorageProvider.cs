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

        public Stream GetWriteStream(string fileName)
        {
            throw new NotImplementedException();
        }

        public Stream GetReadStream(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFile(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
