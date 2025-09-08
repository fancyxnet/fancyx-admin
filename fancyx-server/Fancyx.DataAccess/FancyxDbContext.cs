using Microsoft.EntityFrameworkCore;

namespace Fancyx.DataAccess
{
    public class FancyxDbContext : DbContext
    {

        public FancyxDbContext(DbContextOptions<FancyxDbContext> options) : base(options)
        {
        }
    }
}