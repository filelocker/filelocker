using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Threading;

namespace Filelocker.Services
{
    public class StubSet<T> : EnumerableQuery<T>, IAsyncQueryProvider 
        where T : class
    {
        public StubSet(IEnumerable<T> collection) : base(collection)
        {
            Local = new ObservableCollection<T>(collection);
        }

        public ObservableCollection<T> Local { get; private set; }

        public T Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public T Add(T entity)
        {
            Local.Add(entity);
            return entity;
        }

        public T Remove(T entity)
        {
            Local.Remove(entity);
            return entity;
        }

        public T Attach(T entity)
        {
            return Add(entity);
        }

        public T Create()
        {
            throw new NotImplementedException();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            throw new NotImplementedException();
        }

        public void DeleteObject(T entity)
        {
            throw new NotImplementedException();
        }

        public void Detach(T entity)
        {
            throw new NotImplementedException();
        }

        //async Task<object> IDbAsyncQueryProvider.ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        //{
        //    return ((IQueryProvider)this).Execute(expression);
        //}

        //async Task<TResult> IDbAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        //{
        //    return ((IQueryProvider)this).Execute<TResult>(expression);
        //}

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
