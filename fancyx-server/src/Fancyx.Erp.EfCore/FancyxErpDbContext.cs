using Fancyx.Erp.EfCore.Entites;
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
    }
}
