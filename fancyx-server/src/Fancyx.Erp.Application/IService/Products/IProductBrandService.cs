using Fancyx.Core.Interfaces;
using Fancyx.Erp.Application.IService.Products.Dtos;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.Products
{
    public interface IProductBrandService : IScopedDependency
    {
        Task AddProductBrandAsync(ProductBrandDto dto);
        Task<PagedResult<ProductBrandListDto>> GetProductBrandListAsync(ProductBrandQueryDto dto);
        Task UpdateProductBrandAsync(ProductBrandDto dto);
        Task DeleteProductBrandAsync(long id);
    }
}