using Fancyx.Admin.EfCore.Entities.Organization;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Shared.EfCore;

using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.EfCore
{
    public class FancyxDbContext : AbstractEfCoreDbContext
    {
        public FancyxDbContext(DbContextOptions<FancyxDbContext> options, IServiceProvider serviceProvider) : base(options, serviceProvider)
        {
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
        public DbSet<TenantMenu> TenantMenu { get; set; }
    }
}