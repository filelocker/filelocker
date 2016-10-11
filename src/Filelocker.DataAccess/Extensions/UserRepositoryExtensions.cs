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
        public static async Task<ApplicationUser[]> SearchByName(this IGenericRepository<ApplicationUser> repo, string name)
        {
            var query = repo.AsQueryable().Where(u => u.DisplayName.ToLower().Contains(name.ToLower()));

            return await query.ToArrayAsync();
        }
    }
}
