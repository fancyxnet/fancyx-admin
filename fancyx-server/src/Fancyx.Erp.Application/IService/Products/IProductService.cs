using Fancyx.Core.Interfaces;
using Fancyx.Erp.Application.IService.Products.Dtos;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.Products
{
    public interface IProductService : IScopedDependency
    {
        Task AddProductAsync(ProductDto dto);
        Task<PagedResult<ProductListDto>> GetProductListAsync(ProductQueryDto dto);
        Task UpdateProductAsync(ProductUpdateDto dto);
        Task DeleteProductAsync(long id);
    }
}