using System.Data;
using System.Linq.Expressions;
using Dapper;
using Fancyx.EfCore.BaseEntity;
using Fancyx.EfCore.Models;
using Fancyx.SnowflakeId;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.EfCore
{
    public static class EfCoreExtension
    {
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

        public static Task SoftDeleteAsync<TEntity>(this IQueryable<TEntity> entities, long? userId) where TEntity : IHasDeletionProperty
        {
            return entities.ExecuteUpdateAsync(e => e.SetProperty(s => s.IsDeleted, true)
                .SetProperty(s => s.DeletionTime, DateTime.Now)
                .SetProperty(s => s.DeleterId, userId));
        }

        public static void SetTreeProperties<TEntity>(this TEntity entity, TEntity? parent) where TEntity : Entity<long>, IHasTreeProperty<long?>
        {
            if (entity.Id == default)
            {
                entity.Id = IdGenerater.Instance.NextId();
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

        public static async Task<EntityPaged<TEntity>> PagedAsync<TEntity>(this IQueryable<TEntity> query, int current, int pageSize) where TEntity : class
        {
            return new EntityPaged<TEntity>()
            {
                Total = await query.AsNoTracking().CountAsync(),
                Items = await query.AsNoTracking().Skip((current - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }

        public static async Task<EntityPaged<TModel>> QueryListFromSqlAsync<TModel>(this IDbConnection connection, int current, int pageSize, string sql, DynamicParameters? parameters = null)
        {
            var alias = $"M_{Guid.NewGuid():N}";
            var countSql = $"select count(*) from ({sql}) as {alias}";
            var pagingSql = $"select * from ({sql}) as {alias} limit {pageSize} offset {(current - 1) * pageSize}";
            var total = await connection.ExecuteScalarAsync<int>(countSql, parameters);
            if (total == 0) return new EntityPaged<TModel>();

            var items = (await connection.QueryAsync<TModel>(pagingSql, parameters)).ToList();
            return new EntityPaged<TModel>(total, items);
        }
    }
}