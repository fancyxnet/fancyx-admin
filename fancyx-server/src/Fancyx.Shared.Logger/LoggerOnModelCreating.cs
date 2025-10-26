using Fancyx.EfCore;
using Fancyx.Shared.Logger.Entities;

using Microsoft.EntityFrameworkCore;

namespace Fancyx.Shared.Logger
{
    public class LoggerOnModelCreating : IDbContextOnModelCreating
    {
        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiAccessLog>();
            modelBuilder.Entity<ExceptionLog>();
            modelBuilder.Entity<LogRecord>();
        }
    }
}
