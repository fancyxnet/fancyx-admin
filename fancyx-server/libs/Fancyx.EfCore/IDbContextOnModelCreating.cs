using Microsoft.EntityFrameworkCore;

namespace Fancyx.EfCore
{
    public interface IDbContextOnModelCreating
    {
        void OnModelCreating(ModelBuilder modelBuilder);
    }
}
