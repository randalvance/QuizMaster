using System;
using System.Linq.Expressions;
using System.Reflection;

namespace QuizMaster.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            return ExpressionParser.GetValueFromProperty(obj, propertyName);
        }

        public static object GetPropertyValue(this object obj, LambdaExpression propertyAccessor, Type type)
        {
            return ExpressionParser.GetValueFromProperty(obj, propertyAccessor, type);
        }
    }
}
