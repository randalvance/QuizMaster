using Microsoft.EntityFrameworkCore;
using QuizMaster.Common;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace QuizMaster.Data.Core
{
    public class IncludesCreator<T> where T : class
    {
        public IQueryable<T> ApplyIncludes(IQueryable<T> source, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> result = source;

            foreach (var include in includes)
            {
                var expressions = ExpressionParser.GetFuncsFromExpression(include);
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
    }
}
