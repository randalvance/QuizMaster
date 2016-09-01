using QuizMaster.Common;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace QuizMaster.Extensions
{
    public static class ObjectExtensions
    {
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            var property = obj.GetType().GetTypeInfo().GetProperty(propertyName);

            return property.GetValue(obj);
        }

        public static object GetPropertyValue<T>(this T obj, Expression<Func<T, object>> propertyAccessor)
        {
            return ExpressionParser.GetValueFromProperty(obj, propertyAccessor);
        }
    }
}
