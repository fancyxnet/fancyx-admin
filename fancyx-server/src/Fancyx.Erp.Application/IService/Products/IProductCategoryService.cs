using Fancyx.Core.Interfaces;
using Fancyx.Erp.Application.IService.Products.Dtos;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.Products
{
    public interface IProductCategoryService : IScopedDependency
    {
        Task AddProductCategoryAsync(ProductCategoryDto dto);

        Task<PagedResult<ProductCategoryListDto>> GetProductCategoryListAsync(ProductCategoryQueryDto dto);

        Task UpdateProductCategoryAsync(ProductCategoryDto dto);

        Task DeleteProductCategoryAsync(long id);
    }
}