using Filelocker.DataAccess.Repositories;
using Filelocker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Filelocker.Domain.Interfaces;

namespace Filelocker.DataAccess
{
    public class EfUnitOfWork : DbContext, IUnitOfWork
    {
        private readonly EfGenericRepository<File> _fileRepo;
        private readonly EfGenericRepository<User> _userRepo;

        public DbSet<File> Files { get; set; }
        public DbSet<User> Users { get; set; }

        public EfUnitOfWork()
        {
            _fileRepo = new EfGenericRepository<File>(Files);
            _userRepo = new EfGenericRepository<User>(Users);
        }

        #region IUnitOfWork Implementation

        public IGenericRepository<File> FileRepository => _fileRepo;

        public IGenericRepository<User> UserRepository => _userRepo;

        public async Task CommitAsync()
        {
            await SaveChangesAsync();
        }

        #endregion
    }
}
