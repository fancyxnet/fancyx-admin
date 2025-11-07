using System.Linq.Expressions;

namespace Fancyx.EfCore
{
    /// <summary>
    /// 通用仓储类
    /// 对于已知的多行查询不跟踪，未知的或单行查询跟踪
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        Task<int> InsertAsync(T entity, bool autoSave = true);

        Task<int> InsertManyAsync(List<T> entities, bool autoSave = true);

        IQueryable<T> GetQueryable();

        IQueryable<T> Where(Expression<Func<T, bool>> whereExpression);

        Task<T?> GetAsync(Expression<Func<T, bool>> whereExpression);

        ValueTask<T?> FindAsync<TKey>(TKey id);

        Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression);

        Task<List<T>> GetListAsync();

        Task<bool> AnyAsync(Expression<Func<T, bool>> whereExpression);

        Task<int> CountAsync(Expression<Func<T, bool>> whereExpression);

        Task<int> UpdateAsync(T entity, bool autoSave = true);

        Task<int> UpdateManyAsync(List<T> entities, bool autoSave = true);

        Task<int> DeleteAsync(Expression<Func<T, bool>> whereExpression);

        Task<int> DeleteAsync(T entity, bool autoSave = true);

        IQueryable<T> AsNoTracking();
    }
}