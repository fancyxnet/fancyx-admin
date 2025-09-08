using Fancyx.Core.Authorization;
using Fancyx.Core.Interfaces;
using Fancyx.DataAccess.Entities.Job;
using Fancyx.DataAccess.Entities.Log;
using Fancyx.DataAccess.Entities.Organization;
using Fancyx.DataAccess.Entities.Payment;
using Fancyx.DataAccess.Entities.System;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fancyx.DataAccess
{
    public class FancyxDbContext : DbContext
    {
        private static readonly Type _softDeleteType = typeof(IDeletionProperty);
        private static readonly Type _tenantType = typeof(ITenant);

        public FancyxDbContext(DbContextOptions<FancyxDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyQueryFilter(modelBuilder);
        }

        private static void ApplyQueryFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                LambdaExpression? combinedFilter = null;
                if (_softDeleteType.IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(IDeletionProperty.IsDeleted));
                    var condition = Expression.Equal(property, Expression.Constant(false));
                    if (combinedFilter == null)
                    {
                        combinedFilter = Expression.Lambda(condition, parameter);
                    }
                    else
                    {
                        combinedFilter = Expression.And(combinedFilter, Expression.Lambda(condition, parameter)).Conversion;
                    }
                }
                if (MultiTenancyConsts.IsEnabled && !string.IsNullOrEmpty(TenantManager.Current) && _tenantType.IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(ITenant.TenantId));
                    var condition = Expression.Equal(property, Expression.Constant(TenantManager.Current));
                    if (combinedFilter == null)
                    {
                        combinedFilter = Expression.Lambda(condition, parameter);
                    }
                    else
                    {
                        combinedFilter = Expression.And(combinedFilter, Expression.Lambda(condition, parameter)).Conversion;
                    }
                }
                if (combinedFilter == null) continue;
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(combinedFilter);
            }
        }

        public DbSet<ScheduledTask> ScheduledTask { get; set; }
        public DbSet<TaskExecutionLog> TaskExecutionLog { get; set; }
        public DbSet<ApiAccessLog> ApiAccessLog { get; set; }
        public DbSet<ExceptionLog> ExceptionLog { get; set; }
        public DbSet<LoginLog> LoginLog { get; set; }
        public DbSet<LogRecord> LogRecord { get; set; }
        public DbSet<Dept> Dept { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<PositionGroup> PositionGroup { get; set; }
        public DbSet<PaymentOrder> PaymentOrder { get; set; }
        public DbSet<PayProvider> PayProvider { get; set; }
        public DbSet<Config> Config { get; set; }
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