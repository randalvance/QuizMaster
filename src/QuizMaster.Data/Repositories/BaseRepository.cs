using Microsoft.EntityFrameworkCore;
using QuizMaker.Data.Abstractions;
using QuizMaker.Data.Core;
using QuizMaker.Data.Extensions;
using QuizMaster.Data;
using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Data.Repositories
{
    public class BaseRepository<T> : IRepository<T, Guid> where T : class
    {
        protected ApplicationDbContext DbContext { get; set; }
        protected DbSet<T> DbSet { get; set; }

        private IncludesCreator<T> includesCreator;

        public BaseRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();

            includesCreator = new IncludesCreator<T>();
        }

        public IQueryable<T> RetrieveAll(ListOptions<T> listOptions = null)
        {
            var result = listOptions != null ? includesCreator.ApplyIncludes(DbSet, listOptions.Includes.ToArray()) : DbSet;

            return result;
        }

        public Task<IQueryable<T>> RetrievAllAsync(ListOptions<T> listOptions = null)
        {
            return Task.Run(() =>
            {
                return RetrieveAll(listOptions);
            });
        }

        public Task<T> RetrieveAsync(Guid id, ListOptions<T> listOptions = null)
        {
            return Task.Run(() =>
            {
                var result = listOptions != null ? includesCreator.ApplyIncludes(DbSet, listOptions.Includes.ToArray()) : DbSet;

                return result.Find(id);
            });
        }

        public Task AddAsync(T entity)
        {
            return Task.Run(() =>
            {
                DbSet.Add(entity);
            });
        }

        public Task AddRangeAsync(IEnumerable<T> entitiesToAdd)
        {
            return Task.Run(() =>
            {
                DbSet.AddRange(entitiesToAdd);
            });
        }

        public Task RemoveAsync(T entity)
        {
            return Task.Run(() =>
            {
                DbSet.Remove(entity);
            });
        }

        public async Task RemoveAsync(Guid id)
        {
            var entity = await RetrieveAsync(id);
            await RemoveAsync(entity);
        }

        public Task RemoveRangeAsync(IEnumerable<T> entitiesToRemove)
        {
            return Task.Run(() =>
            {
                DbSet.RemoveRange(entitiesToRemove);
            });
        }

        public Task UpdateAsync(T entity)
        {
            return Task.Run(() =>
            {
                DbSet.Update(entity);
            });
        }

        public async Task CommitAsync()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}
