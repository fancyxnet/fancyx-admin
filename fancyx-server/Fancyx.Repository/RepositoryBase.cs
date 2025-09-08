using Fancyx.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fancyx.Repository
{
    internal class RepositoryBase<T> : IRepository<T> where T : class
    {
        private readonly FancyxDbContext _context;

        public RepositoryBase(FancyxDbContext context)
        {
            _context = context;
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> whereExpression)
        {
            return _context.Set<T>().AnyAsync(whereExpression);
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> whereExpression)
        {
            return _context.Set<T>().CountAsync(whereExpression);
        }

        public Task<int> DeleteAsync(Expression<Func<T, bool>> whereExpression)
        {
            return _context.Set<T>().Where(whereExpression).ExecuteDeleteAsync();
        }

        public Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression)
        {
            return _context.Set<T>().Where(whereExpression).ToListAsync();
        }

        public async Task<EntityPaged<T>> GetPagedListAsync(int current, int pageSize, Expression<Func<T, bool>> whereExpression)
        {
            var query = _context.Set<T>().Where(whereExpression);
            return await query.PagedAsync(current, pageSize);
        }

        public IQueryable<T> GetQueryable()
        {
            return _context.Set<T>();
        }

        public Task<int> InsertAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task<int> InsertManyAsync(List<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            return _context.SaveChangesAsync();
        }

        public Task<T?> GetAsync(Expression<Func<T, bool>> whereExpression)
        {
            return _context.Set<T>().FirstOrDefaultAsync(whereExpression);
        }

        public Task<int> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return _context.SaveChangesAsync();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> whereExpression)
        {
            return _context.Set<T>().Where(whereExpression);
        }

        public Task<List<T>> GetListAsync()
        {
            return _context.Set<T>().ToListAsync();
        }

        public Task<int> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return _context.SaveChangesAsync();
        }
    }
}