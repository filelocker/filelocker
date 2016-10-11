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
        private readonly EfGenericRepository<ApplicationUser> _userRepo;

        public DbSet<File> Files { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }

        public EfUnitOfWork()
        {
            _fileRepo = new EfGenericRepository<File>(Files);
            _userRepo = new EfGenericRepository<ApplicationUser>(Users);
        }

        #region IUnitOfWork Implementation

        public IGenericRepository<File> FileRepository => _fileRepo;

        public IGenericRepository<ApplicationUser> UserRepository => _userRepo;

        public async Task CommitAsync()
        {
            await SaveChangesAsync();
        }

        #endregion
    }
}
