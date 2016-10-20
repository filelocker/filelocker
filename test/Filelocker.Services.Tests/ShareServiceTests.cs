using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Moq;

using Filelocker.Domain;
using Filelocker.Domain.Interfaces;
using Filelocker.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Filelocker.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace Filelocker.Services.Tests
{
    public class ShareServiceTests
    {
        private static DbContextOptions<EfUnitOfWork> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<EfUnitOfWork>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        [Fact]
        public async Task ShareFilePrivatelyAsync_Adds_Shares_To_Repository()
        {
            // Arrange
            var mockUser1 = new ApplicationUser()
            {
                Id = 1
            };
            var mockUser2 = new ApplicationUser()
            {
                Id = 2
            };
            var mockUser3 = new ApplicationUser()
            {
                Id = 3
            };
            var mockUserIds = new[] { mockUser1.Id, mockUser2.Id, mockUser3.Id };
            var mockUserIdsAlreadyShared = new[] { mockUser1.Id };
            var mockFile = new FilelockerFile()
            {
                EncryptionKey = string.Empty,
                EncryptionSalt = Guid.Empty,
                UserId = 3
            };
            var options = CreateNewContextOptions();

            // Run the test against one instance of the context
            using (var context = new EfUnitOfWork(options))
            {
                context.FileRepository.Add(mockFile);
                context.UserRepository.Add(mockUser1);
                context.UserRepository.Add(mockUser2);
                context.UserRepository.Add(mockUser3);
                await context.CommitAsync();
                context.PrivateFileShareRepository.Add(new PrivateFileShare() { ShareTargetId = mockUser1.Id, FilelockerFileId = mockFile.Id });
                await context.CommitAsync();

                var shareService = new ShareService(context);

                // Act
                await shareService.ShareFilePrivatelyAsync(mockFile.Id, mockUserIds);

                // Assert
                var privateShares = await context.PrivateFileShareRepository.GetAllAsync();
                Assert.Equal(3, privateShares.Count());
            } 
        }

        [Fact]
        public async Task GetPrivateSharesAsync_Gets_All_Shares()
        {
            // Arrange
            var mockUser1 = new ApplicationUser()
            {
                Id = 1
            };
            var mockUser2 = new ApplicationUser()
            {
                Id = 2
            };
            var mockFile = new FilelockerFile()
            {
                EncryptionKey = string.Empty,
                EncryptionSalt = Guid.Empty,
                UserId = 3
            };
            
            var options = CreateNewContextOptions();

            // Run the test against one instance of the context
            using (var context = new EfUnitOfWork(options))
            {
                context.FileRepository.Add(mockFile);
                context.UserRepository.Add(mockUser1);
                await context.CommitAsync();

                context.PrivateFileShareRepository.Add(new PrivateFileShare() { ShareTargetId = mockUser1.Id, FilelockerFileId = mockFile.Id });
                context.PrivateFileShareRepository.Add(new PrivateFileShare() { ShareTargetId = mockUser2.Id, FilelockerFileId = mockFile.Id });
                await context.CommitAsync();

                var shareService = new ShareService(context);

                // Act
                var results = await shareService.GetPrivateSharesAsync(mockFile.Id);

                // Assert
                Assert.Equal(2, results.Count());
            }
        }
    }

    public static class MockDbSetExtensions
    {
        public static Mock<DbSet<T>> AsDbSetMock<T>(this IEnumerable<T> list) where T : class
        {
            IQueryable<T> queryableList = list.AsQueryable();
            Mock<DbSet<T>> dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(x => x.Provider).Returns(queryableList.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.Expression).Returns(queryableList.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(queryableList.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(() => queryableList.GetEnumerator());
            return dbSetMock;
        }
    }

    
}
