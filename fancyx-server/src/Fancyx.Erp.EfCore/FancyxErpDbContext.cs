using Fancyx.Erp.EfCore.Entities;
using Fancyx.Shared.EfCore;

using Microsoft.EntityFrameworkCore;

namespace Fancyx.Erp.EfCore
{
    public class FancyxErpDbContext : AbstractEfCoreDbContext
    {
        public FancyxErpDbContext(DbContextOptions<FancyxErpDbContext> options, IServiceProvider serviceProvider) : base(options, serviceProvider)
        {
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<InventoryLog> InventoryLog { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductAttr> ProductAttr { get; set; }
        public DbSet<ProductAttrValue> ProductAttrValue { get; set; }
        public DbSet<ProductBindAttrValue> ProductBindAttrValue { get; set; }
        public DbSet<ProductBrand> ProductBrand { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
    }
}
