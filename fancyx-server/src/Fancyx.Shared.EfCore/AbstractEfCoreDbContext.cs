
using System.Linq.Expressions;

using Fancyx.Core;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
using Fancyx.Utils.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fancyx.Shared.EfCore
{
    public abstract class AbstractEfCoreDbContext : EfCoreDbContextBase
    {
        private static readonly Type _tenantType = typeof(ITenant);
        private readonly ICurrentTenant _currentTenant;

        protected AbstractEfCoreDbContext(DbContextOptions options, IServiceProvider serviceProvider) : base(options, serviceProvider)
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
                    var tenantProviderExpression = Expression.Call(Expression.Constant(this), typeof(AbstractEfCoreDbContext).GetMethod(nameof(GetCurrentTenantId))!);
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
    }
}
