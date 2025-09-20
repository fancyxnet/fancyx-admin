using Fancyx.Shared.EfCore;

using Microsoft.EntityFrameworkCore;

namespace Fancyx.Erp.EfCore
{
    public class FancyxErpDbContext : AbstractEfCoreDbContext
    {
        public FancyxErpDbContext(DbContextOptions<FancyxErpDbContext> options, IServiceProvider serviceProvider) : base(options, serviceProvider)
        {
        }
    }
}
