using Fancyx.Core.Interfaces;
using Fancyx.Erp.Application.IService.Products.Models;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.Products
{
    public interface IProductAttrService : IScopedDependency
    {
        Task AddProductAttrAsync(AddOrUpdateProductAttrRequest req);
        Task<PagedResult<ProductAttrItem>> GetProductAttrListAsync(GetProductAttrListRequest req);
        Task UpdateProductAttrAsync(AddOrUpdateProductAttrRequest req);
        Task DeleteProductAttrAsync(long id);
        Task<PagedResult<ProductAttrValueItem>> GetProductAttrValueListAsync(GetProductAttrValueListRequest req);
        Task AddProductAttrValueAsync(AddOrUpdateProductAttrValueRequest req);
        Task UpdateProductAttrValueAsync(AddOrUpdateProductAttrValueRequest req);
        Task DeleteProductAttrValueAsync(long id);
    }
}