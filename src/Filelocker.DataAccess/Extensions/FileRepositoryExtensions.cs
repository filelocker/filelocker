using System;
using System.Linq;
using System.Threading.Tasks;
using Filelocker.Domain;
using Filelocker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Filelocker.DataAccess.Extensions
{
    public static class FileRepositoryExtensions
    {
        public static async Task<FilelockerFile> GetByIdWithSharesAsync(this IGenericRepository<FilelockerFile> repo, int id)
        {
            var query = repo.AsQueryable();

            return await query
                .Where(f => f.Id == id)
                .Include(f => f.PrivateFileShares)
                .SingleOrDefaultAsync();
        }
    }
}
