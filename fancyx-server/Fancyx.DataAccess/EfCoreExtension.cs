using Fancyx.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fancyx.DataAccess
{
    public static class EfCoreExtension
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> expression)
        {
            if (condition) return query.Where(expression);
            return query;
        }

        public static async Task<TProperty?> ToOneAsync<TEntity, TProperty>(this IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> selector)
        {
            return await query.Select(selector).FirstOrDefaultAsync();
        }

        public static async Task<List<TModel>> SelectToListAsync<TEntity, TModel>(this IQueryable<TEntity> query, Expression<Func<TEntity, TModel>> selector)
        {
            return await query.Select(selector).ToListAsync();
        }

        public static async Task<EntityPaged<TEntity>> PagedAsync<TEntity>(this IQueryable<TEntity> query, int current, int pageSize)
        {
            return new EntityPaged<TEntity>()
            {
                Total = await query.CountAsync(),
                Items = await query.Skip((current - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }
    }
}