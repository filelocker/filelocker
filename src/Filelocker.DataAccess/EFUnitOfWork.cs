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
        private readonly EfGenericRepository<FilelockerFile> _fileRepo;
        private readonly EfGenericRepository<ApplicationUser> _userRepo;
        private readonly EfGenericRepository<PrivateFileShare> _privateShareRepo;


        public DbSet<FilelockerFile> Files { get; set; }

        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<PrivateFileShare> PrivateFileShares { get; set; }

        public EfUnitOfWork(DbContextOptions options) : base(options)
        {
            _fileRepo = new EfGenericRepository<FilelockerFile>(Files);
            _userRepo = new EfGenericRepository<ApplicationUser>(Users);
            _privateShareRepo = new EfGenericRepository<PrivateFileShare>(PrivateFileShares);
        }

        #region IUnitOfWork Implementation

        public IGenericRepository<FilelockerFile> FileRepository => _fileRepo;

        public IGenericRepository<ApplicationUser> UserRepository => _userRepo;

        public IGenericRepository<PrivateFileShare> PrivateFileShareRepository => _privateShareRepo;

        public async Task CommitAsync()
        {
            await SaveChangesAsync();
        }

        #endregion
    }
}
