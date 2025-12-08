using Fancyx.Erp.Application.IService.Products;
using Fancyx.Erp.Application.IService.Products.Models;
using Fancyx.Shared.Models;
using Fancyx.Shared.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Erp.Controllers.Products
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoryController : ControllerBase
    {
        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            ProductCategoryService = productCategoryService;
        }

        public IProductCategoryService ProductCategoryService { get; }

        [HttpPost("Add")]
        [HasPermission("Erp.ProductCategory.Add")]
        public async Task<AppResponse<bool>> AddProductCategoryAsync([FromBody] AddOrUpdateProductCategory req)
        {
            await ProductCategoryService.AddProductCategoryAsync(req);
            return Result.Ok();
        }

        [HttpPost("List")]
        [HasPermission("Erp.ProductCategory.List")]
        public async Task<AppResponse<PagedResult<ProductCategoryItem>>> GetProductCategoryListAsync([FromQuery] GetProductCategoryListRequest req)
        {
            var data = await ProductCategoryService.GetProductCategoryListAsync(req);
            return Result.Data(data);
        }

        [HttpPost("Update")]
        [HasPermission("Erp.ProductCategory.Update")]
        public async Task<AppResponse<bool>> UpdateProductCategoryAsync([FromBody] AddOrUpdateProductCategory req)
        {
            await ProductCategoryService.UpdateProductCategoryAsync(req);
            return Result.Ok();
        }

        [HttpPost("Delete/{id}")]
        [HasPermission("Erp.ProductCategory.Delete")]
        public async Task<AppResponse<bool>> DeleteProductCategoryAsync(long id)
        {
            await ProductCategoryService.DeleteProductCategoryAsync(id);
            return Result.Ok();
        }
    }
}