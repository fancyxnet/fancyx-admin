using Dapper;
using Fancyx.Core.AutoInject;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
using Fancyx.EfCore.Models;
using Fancyx.Erp.EfCore.Entites;
using Fancyx.Erp.EfCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.Erp.EfCore.Repositories
{
    [DenpendencyInject(AsSelf = true)]
    public class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser)
        {
        }

        public Task<EntityPaged<ProductItem>> QueryProductListAsync(int current, int pageSize, string? name)
        {
            // TODO: 
            var sql = "";
            var parameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(name))
            {
                parameters.Add("name", name);
            }
            return Connection.QueryListFromSqlAsync<ProductItem>(current, pageSize, sql, parameters);
        }
    }
}