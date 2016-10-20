using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Xunit;
using Moq;

using Filelocker.Api.Controllers;
using Filelocker.Domain;
using Filelocker.Domain.Interfaces;

namespace Filelocker.Api.Tests
{
    public class FileControllerTests
    {
        [Fact]
        public async Task GetFileTest()
        {
            // Arrange
            var envMock = new Mock<IHostingEnvironment>();
            var mockFileService = new Mock<IFileService>();
            var mockFile = new FilelockerFile()
            {
                EncryptionKey = string.Empty,
                EncryptionSalt = Guid.Empty
            };
            mockFileService.Setup(s => s.GetFileByIdAsync(It.IsAny<int>())).ReturnsAsync(mockFile);
            mockFileService.Setup(f => f.GetReadStream(It.IsAny<FilelockerFile>())).Returns(new MemoryStream());
            
            var filesController = new FilesController(envMock.Object, mockFileService.Object);

            // Act
            var result = await filesController.GetAsync(0);

            // Assert
            Assert.NotNull(result);
            mockFileService.Verify(v => v.GetFileByIdAsync(It.IsAny<int>())); 
            mockFileService.Verify(v => v.GetReadStream(It.IsAny<FilelockerFile>()));
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
