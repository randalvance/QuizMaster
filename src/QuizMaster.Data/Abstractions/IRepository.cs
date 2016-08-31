using QuizMaster.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QuizMaster.Data.Abstractions
{
    interface IRepository<T, K> where T : class
    {
        IQueryable<T> RetrieveAll(ListOptions<T> listOptions = null);
        Task<IQueryable<T>> RetrievAllAsync(ListOptions<T> listOptions = null);
        //Task<T> RetrieveAsync(K id, ListOptions<T> listOptions = null);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entitiesToAdd);
        Task UpdateAsync(T entity);
        Task RemoveAsync(K id);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entitiesToRemove);
        Task CommitAsync();
    }
}
