using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Moq;

using Filelocker.Domain;
using Filelocker.Domain.Interfaces;

namespace Filelocker.Services.Tests
{
    public class FileServiceTests
    {
        [Fact]
        public async Task GetFileByIdTest()
        {
            // Arrange
            var mockFile = new FilelockerFile()
            {
                Id = 5,
                EncryptionKey = string.Empty,
                EncryptionSalt = Guid.Empty
            };
            var mockFileRepository = new Mock<IGenericRepository<FilelockerFile>>();
            mockFileRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockFile);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(w => w.FileRepository).Returns(mockFileRepository.Object);
            var mockFileStorageProvider = new Mock<IFileStorageProvider>();

            var fileService = new FileService(mockUnitOfWork.Object, mockFileStorageProvider.Object);

            // Act
            var result = await fileService.GetFileByIdAsync(0);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Id, 5);
            mockFileRepository.Verify(v => v.GetByIdAsync(It.IsAny<int>()));
        }
    }
}
