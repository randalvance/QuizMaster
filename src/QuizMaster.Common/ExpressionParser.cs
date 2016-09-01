using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

        public static IEnumerable<LambdaExpression> GetFuncsFromExpression<T>(Expression<Func<T, object>> expression, bool includeIndexers = false)
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
                    ((UnaryExpression)exp.Expression).Type : typeof(T);

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
