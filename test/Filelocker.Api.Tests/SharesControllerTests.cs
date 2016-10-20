using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Xunit;
using Moq;

using Filelocker.Api.Controllers;
using Filelocker.Domain;
using Filelocker.Domain.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Filelocker.Api.Tests
{
    public class SharesControllerTests
    {
        [Fact]
        public async Task GetAsync_Gets_All_Shares()
        {
            // Arrange
            var envMock = new Mock<IHostingEnvironment>();
            var mockShareService = new Mock<IShareService>();
            var mockShares = new List<PrivateFileShare>()
                {
                    new PrivateFileShare()
                    {
                        ShareTargetId = 1,
                        FilelockerFileId = 0,
                    }
                }.ToArray();
            mockShareService.Setup(s => s.GetPrivateSharesAsync(It.IsAny<int>())).ReturnsAsync(mockShares);
            
            var sharesController = new SharesController(envMock.Object, mockShareService.Object);

            // Act
            var result = await sharesController.GetAsync(0);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull((((ObjectResult)result).Value as PrivateFileShare[]));
            Assert.Equal(mockShares.Length, (((ObjectResult)result).Value as PrivateFileShare[]).Length);
            mockShareService.Verify(v => v.GetPrivateSharesAsync(It.IsAny<int>())); 
        }

        [Fact]
        public async Task DeleteFileTest()
        {
            // Arrange
            var envMock = new Mock<IHostingEnvironment>();
            var mockFileService = new Mock<IFileService>();
            var filesController = new FilesController(envMock.Object, mockFileService.Object);

            // Act
            await filesController.DeleteAsync(0);

            // Assert
            mockFileService.Verify(v => v.DeleteFileAsync(It.IsAny<int>())); 

        }
    }
}
