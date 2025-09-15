using Fancyx.Core.Expressions;
using Fancyx.Core.Interfaces;
using Fancyx.DataAccess.BaseEntity;
using Fancyx.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Fancyx.DataAccess
{
    public static class EfCoreExtension
    {
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> expression) where T : class
        {
            if (condition) return query.Where(expression);
            return query;
        }

        public static async Task<TProperty?> ToOneAsync<TEntity, TProperty>(this IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> selector) where TEntity : class
        {
            return await query.Select(selector).FirstOrDefaultAsync();
        }

        public static async Task<List<TModel>> SelectToListAsync<TEntity, TModel>(this IQueryable<TEntity> query, Expression<Func<TEntity, TModel>> selector) where TEntity : class
        {
            return await query.Select(selector).ToListAsync();
        }

        public static async Task<EntityPaged<TEntity>> PagedAsync<TEntity>(this IQueryable<TEntity> query, int current, int pageSize) where TEntity : class
        {
            return new EntityPaged<TEntity>()
            {
                Total = await query.AsNoTracking().CountAsync(),
                Items = await query.AsNoTracking().Skip((current - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }

        public static Task SoftDeleteAsync<TEntity>(this IQueryable<TEntity> entities, Guid? userId) where TEntity : FullAuditedEntity
        {
            var now = DateTime.Now;
            return entities.ExecuteUpdateAsync(e => e.SetProperty(s => s.IsDeleted, true)
                .SetProperty(s => s.DeletionTime, now)
                .SetProperty(s => s.DeleterId, userId));
        }

        public static void SetTreeProperties<TEntity>(this TEntity entity, TEntity? parent) where TEntity : Entity, ITreeEntity
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }
            if (parent != null)
            {
                entity.TreePath = $"{parent.TreePath}/{entity.Id}";
                entity.TreeLevel = parent.TreeLevel + 1;
                return;
            }
            entity.TreePath = entity.Id.ToString();
            entity.TreeLevel = 1;
        }

        public static IQueryable<TEntity> PowerFilter<TEntity>(this IQueryable<TEntity> query, ICurrentUser currentUser)
        {
            if (currentUser.IsInRoles(DataPower.SuperAdmin)) return query;

            var type = typeof(TEntity);
            var userIds = currentUser.FindClaim(DataPower.UserIdType)?.Value;
            var deptIds = currentUser.FindClaim(DataPower.DeptIdType)?.Value;
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
                if (field == DataPower.UserId && !string.IsNullOrEmpty(userIds))
                {
                    userFilter = true;
                    filterMap.Add(field, 0);
                    userIdPropName = propName;
                    curFilterExpre = Expression.Constant(userIds, typeof(string));
                }
                if (field == DataPower.DeptId && !string.IsNullOrEmpty(deptIds))
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