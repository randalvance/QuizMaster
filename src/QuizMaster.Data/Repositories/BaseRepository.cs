using Microsoft.EntityFrameworkCore;
using QuizMaster.Common;
using QuizMaster.Common.Extensions;
using QuizMaster.Data.Abstractions;
using QuizMaster.Data.Core;
using QuizMaster.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Data.Repositories
{
    public class BaseRepository<T> : IRepository<T, Guid> where T : class
    {
        private ISortManager sortApplier;

        protected ApplicationDbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }

        private IncludesCreator<T> includesCreator;

        public BaseRepository(ApplicationDbContext dbContext, ISortManager sortApplier)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();

            includesCreator = new IncludesCreator<T>();

            this.sortApplier = sortApplier;
        }

        public IQueryable<T> RetrieveAll(ListOptions<T> listOptions = null)
        {
            if (listOptions == null)
            {
                return DbSet;
            }

            var result = includesCreator.ApplyIncludes(DbSet, listOptions.Includes.ToArray());

            if (listOptions.PagingAndSorting == null)
            {
                return result;
            }

            if (!string.IsNullOrWhiteSpace(listOptions.PagingAndSorting.SortExpression))
            {
                result = sortApplier.ApplySorting(listOptions.PagingAndSorting.SortExpression, result);
            }

            var page = listOptions.PagingAndSorting.Page;
            var itemsPerPage = listOptions.PagingAndSorting.ItemsPerPage;

            result = listOptions != null ? result.Skip(itemsPerPage * (page < 0 ? 0 : page - 1)).Take(itemsPerPage) : result;

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

        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }
    }
}
