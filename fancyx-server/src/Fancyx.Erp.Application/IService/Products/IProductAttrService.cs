using Fancyx.Core.Interfaces;
using Fancyx.Erp.Application.IService.Products.Dtos;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.Products
{
    public interface IProductAttrService : IScopedDependency
    {
        Task AddProductAttrAsync(ProductAttrDto dto);
        Task<PagedResult<ProductAttrListDto>> GetProductAttrListAsync(ProductAttrQueryDto dto);
        Task UpdateProductAttrAsync(ProductAttrDto dto);
        Task DeleteProductAttrAsync(long id);
        Task<PagedResult<ProductAttrValueListDto>> GetProductAttrValueListAsync(ProductAttrValueQueryDto dto);
        Task AddProductAttrValueAsync(ProductAttrValueDto dto);
        Task UpdateProductAttrValueAsync(ProductAttrValueDto dto);
        Task DeleteProductAttrValueAsync(long id);
    }
}