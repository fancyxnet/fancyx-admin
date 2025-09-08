using Microsoft.EntityFrameworkCore;

namespace Fancyx.Repository
{
    public class FancyxDbContext : DbContext
    {
        public FancyxDbContext(DbContextOptions<FancyxDbContext> options) : base(options)
        {
        }
    }
}