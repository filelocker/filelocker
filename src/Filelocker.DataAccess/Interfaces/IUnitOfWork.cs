using Filelocker.Domain;
using System;
using System.Threading.Tasks;

namespace Filelocker.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<File> FileRepository { get; }
        IGenericRepository<User> UserRepository { get; }

        Task CommitAsync();
    }
}
