using System.Linq.Expressions;

namespace Fancyx.Core.Expressions
{
    public static class ExpressionHelper
    {
        /// <summary>
        /// 将表达式属性转换为字符串表达式，支持Guid转换
        /// </summary>
        public static Expression ConvertPropertyToString(Expression propertyExpression)
        {
            if (propertyExpression.Type == typeof(string))
            {
                return propertyExpression;
            }

            // Guid类型
            if (propertyExpression.Type == typeof(Guid))
            {
                var toStringMethod = typeof(Guid).GetMethod("ToString", new Type[0]);
                return Expression.Call(propertyExpression, toStringMethod!);
            }

            // Nullable<Guid>类型
            if (propertyExpression.Type == typeof(Guid?))
            {
                return ConvertNullableGuidToString(propertyExpression);
            }

            // 其他类型调用通用的ToString()
            var genericToStringMethod = typeof(object).GetMethod("ToString");
            return Expression.Call(propertyExpression, genericToStringMethod!);
        }

        /// <summary>
        /// 处理Nullable<Guid>到字符串的转换
        /// </summary>
        private static Expression ConvertNullableGuidToString(Expression propertyExpression)
        {
            var valueProperty = typeof(Guid?).GetProperty("Value");
            var hasValueProperty = typeof(Guid?).GetProperty("HasValue");

            var hasValueExpr = Expression.Property(propertyExpression, hasValueProperty!);
            var valueExpr = Expression.Property(propertyExpression, valueProperty!);

            var toStringMethod = typeof(Guid).GetMethod("ToString", new Type[0]);
            var toStringExpr = Expression.Call(valueExpr, toStringMethod!);

            // 三元表达式：HasValue ? Value.ToString() : null
            return Expression.Condition(
                hasValueExpr,
                toStringExpr,
                Expression.Constant(null, typeof(string))
            );
        }

        /// <summary>
        /// 创建包含表达式，自动处理类型转换
        /// </summary>
        public static Expression<Func<T, bool>> CreateContainsExpression<T>(
            string propertyName,
            string searchValue,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, propertyName);

            // 将属性转换为字符串表达式
            var stringProperty = ConvertPropertyToString(property);

            // 创建常量表达式
            var constant = Expression.Constant(searchValue, typeof(string));

            // 获取String.Contains方法
            var containsMethod = typeof(string).GetMethod(
                "Contains",
                new[] { typeof(string), typeof(StringComparison) });

            // 调用Contains方法
            var containsCall = Expression.Call(
                stringProperty,
                containsMethod!,
                constant,
                Expression.Constant(comparison));

            return Expression.Lambda<Func<T, bool>>(containsCall, parameter);
        }
    }
}