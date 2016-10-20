﻿using System;
using System.Threading.Tasks;

namespace Filelocker.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<FilelockerFile> FileRepository { get; }

        IGenericRepository<ApplicationUser> UserRepository { get; }

        IGenericRepository<PrivateFileShare> PrivateFileShareRepository { get; }

        Task CommitAsync();
    }
}
