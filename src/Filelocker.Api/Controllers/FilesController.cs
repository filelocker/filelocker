using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Filelocker.Api.Controllers
{
    [Route("api/[controller]")]
    public class FilesController
    {
        private readonly IHostingEnvironment _environment;

        public FilesController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet]
        public async Task<FileResult> GetAsync(int id)
        {
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
            return new CreatedAtRouteResult("GetAsync", new {id = 0});

            //    return CreatedAtRoute(
            //routeName: "SubscriberLink",
            //routeValues: new { id = subscriber.Id },
            //value: subscriber);
        }
    }
}
