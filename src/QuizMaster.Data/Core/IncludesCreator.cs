using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace QuizMaker.Data.Core
{
    public class IncludesCreator<T> where T : class
    {
        public IEnumerable<LambdaExpression> GetFuncsFromExpression(Expression<Func<T, object>> expression)
        {
            MemberExpression exp = null;

            var delegates = new List<LambdaExpression>();

            if (expression.Body is MemberExpression)
            {
                exp = expression.Body as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.Convert || expression.Body.NodeType == ExpressionType.ConvertChecked)
            {
                var ue = expression.Body as UnaryExpression;
                exp = ((ue != null) ? ue.Operand : null) as MemberExpression;
            }

            if (exp == null)
            {
                return delegates;
            }

            Type parentMemberType;

            var types = new List<Type>();

            while (exp != null)
            {
                MemberExpression parentMemberExpression =  exp.Expression != null ? exp.Expression as MemberExpression : null;
                MethodCallExpression indexerExpression = null;

                if (parentMemberExpression == null)
                {
                    indexerExpression = exp.Expression as MethodCallExpression;
                }

                var propertyInfo = parentMemberExpression != null ? parentMemberExpression.Member as PropertyInfo : null;
                parentMemberType = propertyInfo != null ? propertyInfo.PropertyType :
                    indexerExpression != null ? (indexerExpression.Object as MemberExpression).Type.GetGenericArguments().First() : typeof(T);

                var member = exp.Member;
                var ps = Expression.Parameter(parentMemberType);

                delegates.Add(Expression.Lambda(Expression.Property(ps, member.Name), ps));

                exp = (indexerExpression != null ? indexerExpression.Object : exp.Expression) as MemberExpression;
            }

            delegates.Reverse();

            return delegates;
        }

        public IQueryable<T> ApplyIncludes(IQueryable<T> source, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> result = source;

            foreach (var include in includes)
            {
                var expressions = this.GetFuncsFromExpression(include);
                if (expressions.Count() == 0)
                {
                    continue;
                }

                var firstInclude = expressions.First();
                var genericArguments = expressions.First().GetType().GetGenericArguments().First().GetGenericArguments();

                var includableQueryable = IncludeMethodInfo.MakeGenericMethod(genericArguments[0], genericArguments[1])
                                .Invoke(null, new object[] { result, firstInclude });

                var expressionList = expressions.ToList();

                if (expressions.Count() > 1)
                {
                    for (int i = 1; i < expressions.Count(); i++)
                    {
                        var expression = expressionList[i];
                        var genericArgumentsThenIncludable = includableQueryable.GetType().GetGenericArguments();
                        genericArguments = expression.GetType().GetGenericArguments().First().GetGenericArguments();

                        if (typeof(IEnumerable).IsAssignableFrom(genericArgumentsThenIncludable[1]))
                        {
                            includableQueryable = ThenIncludeAfterCollectionMethodInfo.MakeGenericMethod(typeof(T), genericArgumentsThenIncludable[1].GetGenericArguments().First(), genericArguments[1])
                                .Invoke(null, new object[] { includableQueryable, expression });
                        }
                        else
                        {
                            includableQueryable = ThenIncludeAfterReferenceMethodInfo.MakeGenericMethod(typeof(T), genericArgumentsThenIncludable[1], genericArguments[1])
                                .Invoke(null, new object[] { includableQueryable, expression });
                        }
                    }
                }

                result = (IQueryable<T>)includableQueryable;
            }

            return result;
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

        //private class IncludableQueryable<TEntity, TProperty> : IIncludableQueryable<TEntity, TProperty>, IAsyncEnumerable<TEntity>
        //{
        //    private readonly IQueryable<TEntity> _queryable;

        //    public IncludableQueryable(IQueryable<TEntity> queryable)
        //    {
        //        _queryable = queryable;
        //    }

        //    public Expression Expression => _queryable.Expression;
        //    public Type ElementType => _queryable.ElementType;
        //    public IQueryProvider Provider => _queryable.Provider;

        //    public IEnumerator<TEntity> GetEnumerator() => _queryable.GetEnumerator();

        //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //    IAsyncEnumerator<TEntity> IAsyncEnumerable<TEntity>.GetEnumerator()
        //        => ((IAsyncEnumerable<TEntity>)_queryable).GetEnumerator();
        //}
    }
}
