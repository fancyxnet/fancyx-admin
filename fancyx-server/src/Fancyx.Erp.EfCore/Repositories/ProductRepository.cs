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
            var sql = @"select p.id,p.code,p.sku_code,p.name,p.remark,p.is_enabled as IsEnabled,b.name as Brand,c.name as Category,p.unit
                        from product p inner join product_brand b on p.brand_id=p.id
                        inner join product_category c on p.category_id=c.id
                        order by p.creation_time desc";
            var parameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(name))
            {
                parameters.Add("name", name);
            }
            return Connection.QueryListFromSqlAsync<ProductItem>(current, pageSize, sql, parameters);
        }

        public Task<ProductSlimInfo?> GetProductSlimInfoAsync(long id)
        {
            var sql = "select name from product where id = @id";
            return Connection.ExecuteScalarAsync<ProductSlimInfo>(sql, new { id = id });
        }
    }
}