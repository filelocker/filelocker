using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Filelocker.Domain.Interfaces;

namespace Filelocker.Api.Controllers
{
    [Route("api/[controller]")]
    public class FilesController
    {
        private readonly IHostingEnvironment _environment;
        private readonly IUnitOfWork _unitOfwork;

        public FilesController(IHostingEnvironment environment, IUnitOfWork unitOfWork)
        {
            _environment = environment;
            _unitOfwork = unitOfWork;
        }

        [HttpGet]
        public async Task<FileResult> GetAsync(int id)
        {
            var file = await _unitOfwork.FileRepository.GetByIdAsync(id);
            //TODO: Read file stream, decrypt
            var result = new FileContentResult(new byte[0], "application/pdf")
            {
                FileDownloadName = "test.pdf"
            };

            return await Task.FromResult(result);
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
            return new CreatedAtRouteResult("GetAsync", new {id = 0});

            //    return CreatedAtRoute(
            //routeName: "SubscriberLink",
            //routeValues: new { id = subscriber.Id },
            //value: subscriber);
        }
    }
}
