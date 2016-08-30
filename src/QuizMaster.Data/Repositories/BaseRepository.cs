using Microsoft.EntityFrameworkCore;
using QuizMaker.Data.Abstractions;
using QuizMaker.Data.Extensions;
using QuizMaster.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace QuizMaker.Data.Repositories
{
    public class BaseRepository<T> : IRepository<T, Guid> where T : class
    {
        protected ApplicationDbContext DbContext { get; set; }
        protected DbSet<T> DbSet { get; set; }

        public BaseRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        public Task<IEnumerable<T>> ListAsync(IListOptions<T> listOptions = null)
        {
            //return Task.FromResult(DbSet.ToList());
            return Task.FromResult(new List<T>().AsEnumerable());
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
        
        private void CreateIncludeExpression(IListOptions<T> listOptions)
        {
            var expressions = listOptions.Includes;

            foreach (var expression in expressions)
            {
                var funcs = GetFuncsFromExpression(expression);
            }
        }

        private object GetFuncsFromExpression(Expression<Func<T, object>> expression)
        {
            Expression exp = expression;
            var funcGenericType = typeof(Func<,>);
            Type lastType = typeof(T);

            var types = new List<Type>();

            while (exp is MemberExpression)
            {
                var memberExpression = exp as MemberExpression;
                var member = memberExpression.Member;
                var closedType = funcGenericType.MakeGenericType(lastType, member.GetType());
                var ps = Expression.Parameter(lastType);
                var ex1 = Expression.Lambda(Expression.Property(ps, member.Name), ps);

                
                exp = memberExpression.Expression;
            }
        }

        internal static readonly MethodInfo IncludeMethodInfo
            = typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo().GetDeclaredMethods("Include")
                .Single(mi => mi.GetParameters().Any(pi => pi.Name == "navigationPropertyPath"));

        internal static readonly MethodInfo ThenIncludeAfterCollectionMethodInfo
            = typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
                .Single(mi => !mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter);

        internal static readonly MethodInfo ThenIncludeAfterReferenceMethodInfo
            = typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
                .Single(mi => mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter);
    }
}
