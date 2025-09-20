using Fancyx.EfCore;
using Fancyx.Logger.Entities;

using Microsoft.EntityFrameworkCore;

namespace Fancyx.Logger
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
