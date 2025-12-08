using Fancyx.Core.Interfaces;
using Fancyx.Erp.Application.IService.Products.Models;
using Fancyx.Erp.EfCore.Models;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.Products
{
    public interface IProductService : IScopedDependency
    {
        Task AddProductAsync(AddOrUpdateProductRequest req);
        Task<PagedResult<ProductItem>> GetProductListAsync(GetProductListRequest req);
        Task UpdateProductAsync(UpdateProductRequest req);
        Task DeleteProductAsync(long id);
    }
}