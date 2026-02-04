using Cracker.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.Logger
{
    public class LoggerDbContext : EfCoreDbContextBase
    {
        public LoggerDbContext(DbContextOptions<LoggerDbContext> options, IServiceProvider serviceProvider) : base(options, serviceProvider)
        {
        }
    }
}
