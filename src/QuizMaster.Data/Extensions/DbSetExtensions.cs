using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using QuizMaster.Data;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace QuizMaster.Data.Extensions
{
    public static class DbSetExtensions
    {
        public static TEntity Find<TEntity>(this IQueryable<TEntity> set, object id) where TEntity : class
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var query = set.Where((Expression<Func<TEntity, bool>>)
                Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, $"{typeof(TEntity).Name}Id"),
                        Expression.Constant(id)),
                    parameter));
            
            return query.FirstOrDefault();
        }
    }
}

