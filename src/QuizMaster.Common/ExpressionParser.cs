using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace QuizMaster.Common
{
    public static class ExpressionParser
    {
        public static object GetValueFromProperty<T>(T obj, Expression<Func<T, object>> expression)
        {
            var delegates = GetFuncsFromExpression(expression, true);
            var cur = obj as object;

            foreach (LambdaExpression exp in delegates)
            {
                var func = exp.Compile();
                cur = func.DynamicInvoke(cur);
            }

            return cur;
        }

        public static object GetValueFromProperty(object obj, string propertyString)
        {
            if (!propertyString.Trim().Any())
            {
                return null;
            }

            var properties = propertyString.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());

            if (properties.Any(x => x.EndsWith("]")))
            {
                throw new NotSupportedException("Using collection and indexers in property path is not yet supported");
            }

            var cur = obj;

            foreach(var property in properties)
            {
                var propInfo = cur.GetType().GetTypeInfo().GetProperty(property);

                if (propInfo == null)
                {
                    throw new ArgumentException($"Invalid property {property} of type {cur.GetType().Name}");
                }

                cur = propInfo.GetValue(cur);
            }

            return cur;
        }

        public static string GetPropertyStringFromExpression<T>(Expression<Func<T, object>> expression)
        {
            var lambdas = GetFuncsFromExpression(expression);

            var propertyNames = new List<string>();

            foreach(var lambda in lambdas)
            {
                var memberExpression = (MemberExpression)(lambda.Body);

                propertyNames.Add(memberExpression.Member.Name);
            }

            return string.Join(".", propertyNames.ToArray());
        }

        public static IEnumerable<LambdaExpression> GetFuncsFromExpression<T>(Expression<Func<T, object>> expression,bool includeIndexers = false)
        {
            return GetFuncsFromExpression(expression, typeof(T), includeIndexers);
        }

        public static IEnumerable<LambdaExpression> GetFuncsFromExpression(LambdaExpression expression, Type type, bool includeIndexers = false)
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
                Type castType = exp.Expression.NodeType == ExpressionType.TypeAs ?
                    ((UnaryExpression)exp.Expression).Type : type;

                MemberExpression parentMemberExpression =  exp.Expression != null ? exp.Expression as MemberExpression : null;
                MethodCallExpression indexerExpression = null;

                if (parentMemberExpression == null)
                {
                    indexerExpression = exp.Expression as MethodCallExpression;
                }

                var propertyInfo = parentMemberExpression != null ? parentMemberExpression.Member as PropertyInfo : null;
                parentMemberType = propertyInfo != null ? propertyInfo.PropertyType :
                    indexerExpression != null ? (indexerExpression.Object as MemberExpression).Type.GetGenericArguments().First() : castType;

                var member = exp.Member;
                var ps = Expression.Parameter(parentMemberType);

                delegates.Add(Expression.Lambda(Expression.Property(ps, member.Name), ps));
                
                exp = (indexerExpression != null ? indexerExpression.Object : exp.Expression) as MemberExpression;

                if (indexerExpression != null && includeIndexers)
                {
                    var pType = indexerExpression.Object.Type;
                    var ps2 = Expression.Parameter(pType);
                    var pe = indexerExpression.Object;
                    var index = indexerExpression.Arguments.First();

                    var propertyGetter = Expression.Property(ps2, "Item", index);
                    delegates.Add(Expression.Lambda(propertyGetter, ps2));
                }
            }

            delegates.Reverse();

            return delegates;
        }
    }
}
