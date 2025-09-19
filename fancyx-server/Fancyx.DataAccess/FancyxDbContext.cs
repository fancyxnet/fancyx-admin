using Fancyx.Core;
using Fancyx.Core.Authorization;
using Fancyx.Core.Interfaces;
using Fancyx.DataAccess.BaseEntity;
using Fancyx.DataAccess.Entities.Job;
using Fancyx.DataAccess.Entities.Log;
using Fancyx.DataAccess.Entities.Organization;
using Fancyx.DataAccess.Entities.System;
using Fancyx.Utils.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fancyx.DataAccess
{
    public class FancyxDbContext : DbContext
    {
        private static readonly Type _softDeleteType = typeof(FullAuditedEntity);
        private static readonly Type _tenantType = typeof(ITenant);
        private readonly ICurrentTenant _currentTenant;

        public FancyxDbContext(DbContextOptions<FancyxDbContext> options, ICurrentTenant currentTenant) : base(options)
        {
            _currentTenant = currentTenant;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplyDataQueryFilter(modelBuilder);
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

        /// <summary>
        /// 应用查询过滤器
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void ApplyDataQueryFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                LambdaExpression? lambda = null;
                if (_softDeleteType.IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(FullAuditedEntity.IsDeleted));
                    var condition = Expression.Equal(property, Expression.Constant(false));
                    lambda = Expression.Lambda(condition, parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
                if (MultiTenancyConsts.IsEnabled && _tenantType.IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(ITenant.TenantId));
                    var tenantProviderExpression = Expression.Call(Expression.Constant(this), typeof(FancyxDbContext).GetMethod(nameof(GetCurrentTenantId))!);
                    var condition = Expression.Equal(property, tenantProviderExpression);
                    lambda ??= Expression.Lambda(condition, parameter);
                    if (lambda != null)
                    {
                        var filter2 = Expression.Lambda(condition, parameter);
                        var parameter1 = lambda.Parameters[0];
                        lambda = Expression.Lambda(Expression.AndAlso(new SwapVisitor(parameter1, filter2.Parameters[0]).Visit(lambda.Body)!, filter2.Body), filter2.Parameters);
                    }
                }

                if (lambda != null)
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        public string? GetCurrentTenantId()
        {
            return _currentTenant.TenantId;
        }

        public DbSet<ScheduledTask> ScheduledTask { get; set; }
        public DbSet<TaskExecutionLog> TaskExecutionLog { get; set; }
        public DbSet<ApiAccessLog> ApiAccessLog { get; set; }
        public DbSet<ExceptionLog> ExceptionLog { get; set; }
        public DbSet<LoginLog> LoginLog { get; set; }
        public DbSet<LogRecord> LogRecord { get; set; }
        public DbSet<Dept> Dept { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<PositionGroup> PositionGroup { get; set; }
        public DbSet<Config> Config { get; set; }
        public DbSet<Tenant> Tenant { get; set; }
        public DbSet<DictData> DictData { get; set; }
        public DbSet<DictType> DictType { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RoleDept> RoleDept { get; set; }
        public DbSet<RoleMenu> RoleMenu { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
    }
}