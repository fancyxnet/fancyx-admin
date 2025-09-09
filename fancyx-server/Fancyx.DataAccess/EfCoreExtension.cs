using System.Linq.Expressions;

using Fancyx.DataAccess.BaseEntity;
using Fancyx.DataAccess.Models;

using Microsoft.EntityFrameworkCore;

namespace Fancyx.DataAccess
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
    }
}