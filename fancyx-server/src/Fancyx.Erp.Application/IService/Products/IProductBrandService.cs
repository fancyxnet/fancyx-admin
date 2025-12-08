using Fancyx.Core.Interfaces;
using Fancyx.Erp.Application.IService.Products.Models;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.Products
{
    public interface IProductBrandService : IScopedDependency
    {
        Task AddProductBrandAsync(AddOrUpdateProductBrand req);
        Task<PagedResult<ProductBrandItem>> GetProductBrandListAsync(GetProductBrandListRequest req);
        Task UpdateProductBrandAsync(AddOrUpdateProductBrand req);
        Task DeleteProductBrandAsync(long id);
    }
}