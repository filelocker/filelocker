using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Filelocker.Domain.Interfaces;
using System.Security.Cryptography;

namespace Filelocker.Api.Controllers
{
    [Route("api/[controller]")]
    public class FilesController
    {
        private readonly IHostingEnvironment _environment;
        private readonly IUnitOfWork _unitOfwork;
        private readonly IFileStorageProvider _fileStorageProvider;
        public FilesController(IHostingEnvironment environment, IUnitOfWork unitOfWork, IFileStorageProvider fileStorageProvider)
        {
            _environment = environment;
            _unitOfwork = unitOfWork;
            _fileStorageProvider = fileStorageProvider;
        }

        [HttpGet]
        public async Task<FileStreamResult> GetAsync(int id)
        {
            var file = await _unitOfwork.FileRepository.GetByIdAsync(id);
            var fs = _fileStorageProvider.GetReadStream(id.ToString());
            var decryptor = Aes.Create();
            var pdb = new Rfc2898DeriveBytes(file.EncryptionKey, file.EncryptionSalt.ToByteArray());
            decryptor.Key = pdb.GetBytes(32);
            decryptor.IV = pdb.GetBytes(16);
            var cs = new CryptoStream(fs, decryptor.CreateDecryptor(), CryptoStreamMode.Read);
            return new FileStreamResult(cs, "application/octet-stream");
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]string test)
        {

            //var fileId = 0;
            //var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            //foreach (var file in files)
            //{
            //    if (file.Length > 0)
            //    {
            //        using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
            //        {
            //            await file.CopyToAsync(fileStream);
            //        }
            //    }
            //}
            await Task.Delay(1);
            return new CreatedAtRouteResult("GetAsync", new { id = 0 });

            //    return CreatedAtRoute(
            //routeName: "SubscriberLink",
            //routeValues: new { id = subscriber.Id },
            //value: subscriber);
        }
    }
}
