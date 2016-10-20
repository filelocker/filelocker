using Filelocker.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filelocker.Api.Controllers
{
    [Route("api/files/{fileId}/shares")]
    [Authorize]
    public class SharesController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private readonly IShareService _shareService;

        public SharesController(IHostingEnvironment environment, IShareService shareService)
        {
            _environment = environment;
            _shareService = shareService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(int fileId)
        {
            //TODO: Permissions
            var shares = await _shareService.GetPrivateSharesAsync(fileId);
            return new ObjectResult(shares);
        }
    }
}
