using System.Threading.Tasks;
using Filelocker.Api.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Xunit;

namespace Filelocker.Api.Tests
{
    public class FileControllerTests
    {
        [Fact]
        public async Task GetFileTest()
        {
            IHostingEnvironment envMock = new HostingEnvironment();
            var filesController = new FilesController(envMock);
            var result = await filesController.GetAsync(0);

            Assert.NotNull(result);
        }
        
    }
}
