using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;
using System.Linq.Expressions;

namespace Fancyx.EfCore
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly ICurrentUser _currentUser;
        private static readonly Type _fullAuditedEntityType = typeof(FullAuditedEntity);
        private static readonly Type _efCoreExtensionType = typeof(EfCoreExtension);
        private static readonly Type _currentType = typeof(T);

        public BaseRepository(DbContext context, ICurrentUser currentUser)
        {
            Context = context;
            _currentUser = currentUser;
        }

        protected DbContext Context { get; }
        protected IDbConnection Connection => Context.Database.GetDbConnection();

        public Task<bool> AnyAsync(Expression<Func<T, bool>> whereExpression)
        {
            return Context.Set<T>().AsNoTracking().AnyAsync(whereExpression);
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> whereExpression)
        {
            return Context.Set<T>().AsNoTracking().CountAsync(whereExpression);
        }

        public async Task<int> DeleteAsync(Expression<Func<T, bool>> whereExpression)
        {
            var query = Context.Set<T>().AsNoTracking().Where(whereExpression);
            if (_fullAuditedEntityType.IsAssignableFrom(_currentType))
            {
                var softDeleteMethod = _efCoreExtensionType.GetMethod(nameof(EfCoreExtension.SoftDeleteAsync), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (softDeleteMethod != null)
                {
                    softDeleteMethod = softDeleteMethod.MakeGenericMethod(_currentType);
                    var task = (Task?)softDeleteMethod.Invoke(null, [query, _currentUser.Id]);
                    if (task != null)
                    {
                        await task;
                        return -1;
                    }
                }
            }
            return await query.ExecuteDeleteAsync();
        }

        public Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression)
        {
            return Context.Set<T>().AsNoTracking().Where(whereExpression).ToListAsync();
        }

        public IQueryable<T> GetQueryable()
        {
            return Context.Set<T>();
        }

        public async Task<int> InsertAsync(T entity, bool autoSave = true)
        {
            Context.Set<T>().Add(entity);
            return autoSave ? await Context.SaveChangesAsync() : -1;
        }

        public async Task<int> InsertManyAsync(List<T> entities, bool autoSave = true)
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
                await Context.Set<T>().BulkInsertAsync(entities);
                return entities.Count;
            }
            Context.Set<T>().AddRange(entities);
            return autoSave ? await Context.SaveChangesAsync() : -1;
        }

        public Task<T?> GetAsync(Expression<Func<T, bool>> whereExpression)
        {
            return Context.Set<T>().FirstOrDefaultAsync(whereExpression);
        }

        public ValueTask<T?> FindAsync<TKey>(TKey id)
        {
            return Context.Set<T>().FindAsync(id);
        }

        public async Task<int> UpdateAsync(T entity, bool autoSave = true)
        {
            if (Context.Entry(entity).State != EntityState.Modified)
            {
                Context.Set<T>().Update(entity);
            }
            return autoSave ? await Context.SaveChangesAsync() : -1;
        }

        public async Task<int> UpdateManyAsync(List<T> entities, bool autoSave = true)
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
                await Context.Set<T>().BulkUpdateAsync(entities);
                return entities.Count;
            }
            Context.Set<T>().UpdateRange(entities);
            return autoSave ? await Context.SaveChangesAsync() : -1;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> whereExpression)
        {
            return Context.Set<T>().Where(whereExpression);
        }

        public Task<List<T>> GetListAsync()
        {
            return Context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<int> DeleteAsync(T entity, bool autoSave = true)
        {
            if (entity is FullAuditedEntity val)
            {
                val.Delete(_currentUser.Id.GetValueOrDefault());
                var entry2 = Context.Entry(val);
                if (entry2.State == EntityState.Detached)
                {
                    Context.Attach(val);
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
                Context.Set<T>().Remove(entity);
            }
            return autoSave ? await Context.SaveChangesAsync() : -1;
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