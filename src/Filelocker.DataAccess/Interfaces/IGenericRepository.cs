using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Filelocker.DataAccess
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> AsQueryable();

        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> SingleAsync(Expression<Func<T, bool>> predicate);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(int id);

        void Add(T entity);
        void Delete(T entity);
        void Attach(T entity);
    }
}
