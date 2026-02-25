using Cracker.EfCore;
using Fancyx.Shared.Logger;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.Logger
{
    public class LoggerDbContext : EfCoreDbContextBase
    {
        public LoggerDbContext(DbContextOptions<LoggerDbContext> options, IServiceProvider serviceProvider) : base(options, serviceProvider)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new LoggerOnModelCreating().OnModelCreating(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
