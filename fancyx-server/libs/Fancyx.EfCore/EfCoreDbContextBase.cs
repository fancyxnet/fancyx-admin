using Fancyx.Core.Authorization;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fancyx.EfCore
{
    public class EfCoreDbContextBase : DbContext
    {
        private static readonly Type _softDeleteType = typeof(FullAuditedEntity);

        protected IServiceProvider ServiceProvider { get; }

        public EfCoreDbContextBase(DbContextOptions options, IServiceProvider serviceProvider) : base(options)
        {
            ServiceProvider = serviceProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplyDataQueryFilter(modelBuilder);
        }

        protected virtual void ApplyDataQueryFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var lambda = GetSoftDeleteQueryFitler(entityType.ClrType);
                if (lambda == null) continue;
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }

        public override int SaveChanges()
        {
            ApplyAuditValues();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditValues();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditValues();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyAuditValues();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// 应用审计值填充
        /// </summary>
        private void ApplyAuditValues()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                var entity = entry.Entity;
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entity is CreationEntity creationEntity)
                        {
                            if (creationEntity.CreationTime == default)
                            {
                                creationEntity.CreationTime = DateTime.Now;
                            }

                            creationEntity.CreatorId ??= UserManager.Current;
                        }
                        if (entity is ITenant entityWithTenant)
                        {
                            entityWithTenant.TenantId ??= TenantManager.Current;
                        }
                        break;

                    case EntityState.Modified:
                        if (entity is AuditedEntity auditedEntity)
                        {
                            auditedEntity.LastModificationTime = DateTime.Now;
                            auditedEntity.LastModifierId = UserManager.Current;
                        }
                        break;

                    case EntityState.Deleted:
                        if (entity is FullAuditedEntity fullAuditedEntity)
                        {
                            fullAuditedEntity.IsDeleted = true;
                            fullAuditedEntity.DeletionTime = DateTime.Now;
                            fullAuditedEntity.DeleterId = UserManager.Current;
                        }
                        break;
                }
            }
        }

        protected static LambdaExpression? GetSoftDeleteQueryFitler(Type entityType)
        {
            if (!_softDeleteType.IsAssignableFrom(entityType)) return null;

            var parameter = Expression.Parameter(entityType, "e");
            var property = Expression.Property(parameter, nameof(FullAuditedEntity.IsDeleted));
            var condition = Expression.Equal(property, Expression.Constant(false));
            return Expression.Lambda(condition, parameter);
        }
    }
}