using Microsoft.EntityFrameworkCore;
using QuizMaker.Data.Abstractions;
using QuizMaker.Data.Core;
using QuizMaker.Data.Extensions;
using QuizMaster.Data;
using System;
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

        public Task<IQueryable<T>> ListAsync(ListOptions<T> listOptions = null)
        {
            var result = includesCreator.ApplyIncludes(DbSet, listOptions.Includes.ToArray());

            return Task.FromResult(result);
        }

        public Task<T> RetrieveAsync(Guid id)
        {
            return Task.Run(() =>
            {
                return DbSet.Find(id);
            });
        }

        public Task AddAsync(T entity)
        {
            return Task.Run(() =>
            {
                DbSet.Add(entity);
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
