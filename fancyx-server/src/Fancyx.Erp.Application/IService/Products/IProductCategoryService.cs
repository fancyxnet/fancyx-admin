using Fancyx.Core.Interfaces;
using Fancyx.Erp.Application.IService.Products.Models;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.Products
{
    public interface IProductCategoryService : IScopedDependency
    {
        Task AddProductCategoryAsync(AddOrUpdateProductCategory req);

        Task<PagedResult<ProductCategoryItem>> GetProductCategoryListAsync(GetProductCategoryListRequest req);

        Task UpdateProductCategoryAsync(AddOrUpdateProductCategory req);

        Task DeleteProductCategoryAsync(long id);
    }
}