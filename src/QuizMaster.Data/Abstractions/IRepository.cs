using QuizMaker.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QuizMaker.Data.Abstractions
{
    interface IRepository<T, K> where T : class
    {
        IQueryable<T> List(ListOptions<T> listOptions = null);
        Task<T> RetrieveAsync(K id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(K id);
        Task RemoveAsync(T entity);
        Task CommitAsync();
    }
}
