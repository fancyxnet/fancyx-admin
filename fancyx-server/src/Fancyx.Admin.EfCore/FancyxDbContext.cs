using Fancyx.Admin.EfCore.Entities.Organization;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Core;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
using Fancyx.Utils.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Fancyx.Admin.EfCore
{
    public class FancyxDbContext : EfCoreDbContextBase
    {
        private static readonly Type _tenantType = typeof(ITenant);
        private readonly ICurrentTenant _currentTenant;

        public FancyxDbContext(DbContextOptions<FancyxDbContext> options, IServiceProvider serviceProvider) : base(options, serviceProvider)
        {
            _currentTenant = serviceProvider.GetRequiredService<ICurrentTenant>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplyDataQueryFilter(modelBuilder);
        }

        /// <summary>
        /// 应用查询过滤器
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void ApplyDataQueryFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                LambdaExpression? lambda = GetSoftDeleteQueryFitler(entityType.ClrType);
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

        public DbSet<LoginLog> LoginLog { get; set; }
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