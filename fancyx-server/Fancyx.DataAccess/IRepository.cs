using Fancyx.DataAccess.Models;
using System.Linq.Expressions;

namespace Fancyx.DataAccess
{
    public interface IRepository<T> where T : class
    {
        Task<int> InsertAsync(T entity);

        Task<int> InsertManyAsync(List<T> entities);

        IQueryable<T> GetQueryable();

        IQueryable<T> Where(Expression<Func<T, bool>> whereExpression);

        Task<T?> GetAsync(Expression<Func<T, bool>> whereExpression);

        Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression);

        Task<List<T>> GetListAsync();

        Task<EntityPaged<T>> GetPagedListAsync(int current, int pageSize, Expression<Func<T, bool>> whereExpression);

        Task<bool> AnyAsync(Expression<Func<T, bool>> whereExpression);

        Task<int> CountAsync(Expression<Func<T, bool>> whereExpression);

        Task<int> UpdateAsync(T entity);

        Task<int> UpdateManyAsync(List<T> entities);

        Task<int> DeleteAsync(Expression<Func<T, bool>> whereExpression);

        Task<int> DeleteAsync(T entity);
    }
}