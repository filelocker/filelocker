using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Filelocker.Api.Extensions;
using Filelocker.Domain;
using Filelocker.Domain.Interfaces;

namespace Filelocker.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class FilesController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private readonly IFileService _fileService;

        public FilesController(IHostingEnvironment environment, IFileService fileService)
        {
            _environment = environment;
            _fileService = fileService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            //TODO: Permissions
            var file = await _fileService.GetFileByIdAsync(id);
            if (file == null) return NotFound("File record not found");
            var fs = _fileService.GetReadStream(file);
            
            return new FileStreamResult(fs, "application/octet-stream");
        }

        [HttpGet]
        public async Task<IEnumerable<FilelockerFile>> GetAsync()
        {
            var userId = HttpContext.User.Identity.GetUserId();
            var userFiles = await _fileService.GetFilesByUserIdAsync(userId);
            return userFiles;
        }

        [HttpPost]
        public async Task PostAsync(FilelockerFile file)
        {
            //TODO: Permissions
            await _fileService.CreateOrUpdateFileAsync(file);
        }
        
        [HttpDelete("{id:int}")]
        public async Task DeleteAsync(int id)
        {
            //TODO: Permissions
            await _fileService.DeleteFileAsync(id);
        }
    }
}
