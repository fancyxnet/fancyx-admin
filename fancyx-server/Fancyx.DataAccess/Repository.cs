using Fancyx.Core.Interfaces;
using Fancyx.DataAccess.BaseEntity;
using Fancyx.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Fancyx.DataAccess
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly FancyxDbContext _context;
        private readonly ICurrentUser _currentUser;
        private static readonly Type _softDeleteType = typeof(IDeletionProperty);
        private static readonly Type _fullAuditedEntityType = typeof(FullAuditedEntity);

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

        public Task<int> DeleteAsync(Expression<Func<T, bool>> whereExpression)
        {
            if (_fullAuditedEntityType.IsAssignableFrom(typeof(T)))
            {
                return _context.Set<T>().AsNoTracking().Where(whereExpression)
                    .ExecuteUpdateAsync(this.BuilderSoftDeletePropertyCalls(true));
            }
            else if (_softDeleteType.IsAssignableFrom(typeof(T)))
            {
                return _context.Set<T>().AsNoTracking().Where(whereExpression)
                    .ExecuteUpdateAsync(this.BuilderSoftDeletePropertyCalls());
            }
            return _context.Set<T>().AsNoTracking().Where(whereExpression).ExecuteDeleteAsync();
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
            return _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public Task<int> DeleteAsync(T entity)
        {
            if (entity is IDeletionProperty val1)
            {
                if (entity is FullAuditedEntity val2)
                {
                    val2.Delete(_currentUser.Id.GetValueOrDefault());
                    var entry2 = _context.Entry(val2);
                    if (entry2.State == EntityState.Detached)
                    {
                        _context.Attach(val2);
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
                    val1.IsDeleted = true;
                    var entry1 = _context.Entry(val1);
                    if (entry1.State == EntityState.Detached)
                    {
                        _context.Attach(val1);
                        entry1.Property(e => e.IsDeleted).IsModified = true;
                    }
                    else
                    {
                        SoftDeleteBeforeResetOtherProperty(entry1);
                    }
                }
            }
            else
            {
                _context.Set<T>().Remove(entity);
            }
            return _context.SaveChangesAsync();
        }

        private Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> BuilderSoftDeletePropertyCalls(bool isFullAuditedEntity = false)
        {
            // 创建参数表达式
            var parameter = Expression.Parameter(typeof(SetPropertyCalls<T>), "s");
            // 构建方法调用链
            Expression expression = BuildSetPropertyCall(parameter, nameof(IDeletionProperty.IsDeleted), true);
            if (isFullAuditedEntity)
            {
                expression = BuildSetPropertyCall(expression, nameof(FullAuditedEntity.DeletionTime), DateTime.Now);
                expression = BuildSetPropertyCall(expression, nameof(FullAuditedEntity.DeleterId), _currentUser.Id.GetValueOrDefault());
            }
            return Expression.Lambda<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>>(expression, parameter);
        }

        private Expression BuildSetPropertyCall<TValue>(Expression expression, string propertyName, TValue value)
        {
            // 获取泛型 SetProperty 方法
            var setPropertyMethod = typeof(SetPropertyCalls<T>)
                .GetMethods()
                .First(m => m.Name == "SetProperty" &&
                           m.GetGenericArguments().Length == 1 &&
                           m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TValue));

            // 创建属性选择器表达式：e => e.PropertyName
            var entityParam = Expression.Parameter(typeof(T), "e");
            var propertyAccess = Expression.Property(entityParam, propertyName);
            var propertySelector = Expression.Lambda(propertyAccess, entityParam);

            // 创建值常量表达式
            var valueConstant = Expression.Constant(value, typeof(TValue));

            // 构建方法调用：expression.SetProperty(selector, value)
            return Expression.Call(expression, setPropertyMethod, propertySelector, valueConstant);
        }

        private static void SoftDeleteBeforeResetOtherProperty(EntityEntry entry)
        {
            foreach (var property in entry.Properties)
            {
                if (property.Metadata.Name != nameof(IDeletionProperty.IsDeleted) &&
                    property.Metadata.Name != nameof(FullAuditedEntity.DeleterId) &&
                    property.Metadata.Name != nameof(FullAuditedEntity.DeletionTime))
                {
                    property.IsModified = false;
                }
            }
        }
    }
}