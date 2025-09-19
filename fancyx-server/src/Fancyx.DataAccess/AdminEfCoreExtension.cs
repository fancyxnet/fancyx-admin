using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using Fancyx.Utils.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using Fancyx.Admin.EfCore.Models;

namespace Fancyx.Admin.EfCore
{
    public static class AdminEfCoreExtension
    {
        private static readonly MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;

        public static async Task<EntityPaged<TEntity>> PagedAsync<TEntity>(this IQueryable<TEntity> query, int current, int pageSize) where TEntity : class
        {
            return new EntityPaged<TEntity>()
            {
                Total = await query.AsNoTracking().CountAsync(),
                Items = await query.AsNoTracking().Skip((current - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }

        public static IQueryable<TEntity> PowerFilter<TEntity>(this IQueryable<TEntity> query, ICurrentUser currentUser)
        {
            if (currentUser.IsInRoles(DataPower.SuperAdmin)) return query;

            var type = typeof(TEntity);
            var userIds = currentUser.FindClaim(DataPower.UserIdType)?.Value ?? "";
            var deptIds = currentUser.FindClaim(DataPower.DeptIdType)?.Value ?? "";
            var curDeptId = currentUser.FindClaim(DataPower.DeptId)?.Value;

            // 如果包含了本人用户ID，不含本人部门；
            // 说明没有本部门数据权限，只有本人数据权限，对于本人不加部门筛选
            var currentUserId = currentUser.Id.GetValueOrDefault().ToString();
            var isOnlyMe = userIds?.Contains(currentUserId) == true && (string.IsNullOrEmpty(curDeptId) || !deptIds?.Contains(curDeptId) == true);
            // 用户ID属性名
            string? userIdPropName = null;
            // 防止重复过滤，每个权限字段只过滤1次
            var filterMap = new Dictionary<string, byte>();
            foreach (var prop in type.GetProperties())
            {
                var powerFieldAttr = prop.GetCustomAttribute<DataPowerAttribute>();
                if (powerFieldAttr != null)
                {
                    PadExpr(powerFieldAttr.Field, prop.Name);
                    continue;
                }
                PadExpr(prop.Name, prop.Name);
            }

            void PadExpr(string field, string propName)
            {
                if (filterMap.ContainsKey(field)) return;

                var userFilter = false;
                ConstantExpression? curFilterExpre = null;
                if (field == DataPower.UserId)
                {
                    userFilter = true;
                    filterMap.Add(field, 0);
                    userIdPropName = propName;
                    curFilterExpre = Expression.Constant(userIds, typeof(string));
                }
                if (field == DataPower.DeptId)
                {
                    filterMap.Add(field, 0);
                    curFilterExpre = Expression.Constant(deptIds, typeof(string));
                }
                if (curFilterExpre == null) return;

                var parameter = Expression.Parameter(type, "e");
                var property = Expression.Property(parameter, propName);
                var propConvert = ExpressionHelper.ConvertPropertyToString(property);

                var call = Expression.Call(curFilterExpre, containsMethod, propConvert);
                if (!userFilter && isOnlyMe && !string.IsNullOrEmpty(userIdPropName))
                {
                    // 拼接: 或者用户ID等于当前用户ID
                    var userIdExpr = ExpressionHelper.ConvertPropertyToString(Expression.Property(parameter, userIdPropName));
                    var body = Expression.OrElse(Expression.Equal(userIdExpr, Expression.Constant(currentUserId, typeof(string))), call);
                    query = query.Where(Expression.Lambda<Func<TEntity, bool>>(body, parameter));
                    return;
                }
                query = query.Where(Expression.Lambda<Func<TEntity, bool>>(call, parameter));
            }

            return query;
        }
    }
}