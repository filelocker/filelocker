using System;
using System.Linq;
using System.Threading.Tasks;
using Filelocker.Domain;
using Filelocker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Filelocker.DataAccess.Extensions
{
    public static class UserRepositoryExtensions
    {
        public static async Task<User[]> SearchByName(this IGenericRepository<User> repo, string name)
        {
            var query = repo.AsQueryable().Where(u => u.DisplayName.ToLower().Contains(name.ToLower()));

            return await query.ToArrayAsync();
        }
    }
}
