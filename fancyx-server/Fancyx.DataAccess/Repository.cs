using Fancyx.Core.Interfaces;
using Fancyx.DataAccess.BaseEntity;
using Fancyx.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Fancyx.DataAccess
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly FancyxDbContext _context;
        private readonly ICurrentUser _currentUser;
        private static readonly Type _fullAuditedEntityType = typeof(FullAuditedEntity);
        private static readonly Type _efCoreExtensionType = typeof(EfCoreExtension);

        public Repository(FancyxDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> whereExpression)
        {
            return _context.Set<T>().AsNoTracking().AnyAsync(whereExpression);
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> whereExpression)
        {
            return _context.Set<T>().AsNoTracking().CountAsync(whereExpression);
        }

        public async Task<int> DeleteAsync(Expression<Func<T, bool>> whereExpression)
        {
            var query = _context.Set<T>().AsNoTracking().Where(whereExpression);
            if (_fullAuditedEntityType.IsAssignableFrom(typeof(T)))
            {
                var softDeleteMethod = _efCoreExtensionType.GetMethod(nameof(EfCoreExtension.SoftDeleteAsync), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if(softDeleteMethod != null)
                {
                    var count = await query.CountAsync();
                    var task = (Task?)softDeleteMethod.Invoke(null, [query]);
                    if (task != null)
                    {
                        await task;
                        return count;
                    }
                }
            }
            return await query.ExecuteDeleteAsync();
        }

        public Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression)
        {
            return _context.Set<T>().AsNoTracking().Where(whereExpression).ToListAsync();
        }

        public async Task<EntityPaged<T>> GetPagedListAsync(int current, int pageSize, Expression<Func<T, bool>> whereExpression)
        {
            var query = _context.Set<T>().AsNoTracking().Where(whereExpression);
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

        public async Task<int> InsertManyAsync(List<T> entities)
        {
            if (entities.Count > 500)
            {
                var now = DateTime.Now;
                foreach (var entity in entities)
                {
                    if (entity is CreationEntity value)
                    {
                        value.CreatorId ??= _currentUser.Id;
                        if (value.CreationTime == default) value.CreationTime = now;
                    }
                }
                await _context.Set<T>().BulkInsertAsync(entities);
                return entities.Count;
            }
            _context.Set<T>().AddRange(entities);
            return await _context.SaveChangesAsync();
        }

        public Task<T?> GetAsync(Expression<Func<T, bool>> whereExpression)
        {
            return _context.Set<T>().FirstOrDefaultAsync(whereExpression);
        }

        public Task<int> UpdateAsync(T entity)
        {
            if (_context.Entry(entity).State != EntityState.Modified)
            {
                _context.Set<T>().Update(entity);
            }
            return _context.SaveChangesAsync();
        }

        public async Task<int> UpdateManyAsync(List<T> entities)
        {
            if (entities.Count > 500)
            {
                var now = DateTime.Now;
                foreach (var entity in entities)
                {
                    if (entity is AuditedEntity value)
                    {
                        value.LastModifierId ??= _currentUser.Id;
                        if (value.LastModificationTime == default) value.LastModificationTime = now;
                    }
                }
                await _context.Set<T>().BulkUpdateAsync(entities);
                return entities.Count;
            }
            _context.Set<T>().UpdateRange(entities);
            return await _context.SaveChangesAsync();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> whereExpression)
        {
            return _context.Set<T>().Where(whereExpression);
        }

        public Task<List<T>> GetListAsync()
        {
            return _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public Task<int> DeleteAsync(T entity)
        {
            if (entity is FullAuditedEntity val)
            {
                val.Delete(_currentUser.Id.GetValueOrDefault());
                var entry2 = _context.Entry(val);
                if (entry2.State == EntityState.Detached)
                {
                    _context.Attach(val);
                    entry2.Property(e => e.IsDeleted).IsModified = true;
                    entry2.Property(e => e.DeleterId).IsModified = true;
                    entry2.Property(e => e.DeletionTime).IsModified = true;
                }
                else
                {
                    SoftDeleteBeforeResetOtherProperty(entry2);
                }
            }
            else
            {
                _context.Set<T>().Remove(entity);
            }
            return _context.SaveChangesAsync();
        }

        private static void SoftDeleteBeforeResetOtherProperty(EntityEntry entry)
        {
            foreach (var property in entry.Properties)
            {
                if (property.Metadata.Name != nameof(FullAuditedEntity.IsDeleted) &&
                    property.Metadata.Name != nameof(FullAuditedEntity.DeleterId) &&
                    property.Metadata.Name != nameof(FullAuditedEntity.DeletionTime))
                {
                    property.IsModified = false;
                }
            }
        }
    }
}