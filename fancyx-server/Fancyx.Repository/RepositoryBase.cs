using Fancyx.Repository.Models;
using System.Linq.Expressions;

namespace Fancyx.Repository
{
    internal class RepositoryBase<T> : IRepository<T> where T : class
    {
        public Task<bool> AnyAsync(Expression<Func<T, bool>> whereExpression)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> whereExpression)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Expression<Func<T, bool>> whereExpression)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression)
        {
            throw new NotImplementedException();
        }

        public Task<EntityPaged<T>> GetPagedListAsync(int current, int pageSize, Expression<Func<T, bool>> whereExpression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetQueryable()
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertManyAsync(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetAsync(Expression<Func<T, bool>> whereExpression)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> whereExpression)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}