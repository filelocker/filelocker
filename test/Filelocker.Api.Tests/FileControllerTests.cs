using System.Threading.Tasks;
using Filelocker.Api.Controllers;
using Microsoft.AspNetCore.Hosting;
using Xunit;
using Filelocker.Domain.Interfaces;
using Moq;
using Filelocker.Domain;

namespace Filelocker.Api.Tests
{
    public class FileControllerTests
    {
        [Fact]
        public async Task GetFileTest()
        {
            // Arrange
            var envMock = new Mock<IHostingEnvironment>();
            var fileStorageProvider = new Mock<IFileStorageProvider>();
            var fileRepositoryMock = new Mock<IGenericRepository<FilelockerFile>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(foo => foo.FileRepository).Returns(fileRepositoryMock.Object);
            var filesController = new FilesController(envMock.Object, unitOfWorkMock.Object, fileStorageProvider.Object);

            // Act
            var result = await filesController.GetAsync(0);

            // Assert
            Assert.NotNull(result);
        }
        
    }
}
