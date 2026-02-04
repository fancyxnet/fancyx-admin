using AutoMapper;
using Cracker.EfCore;
using Fancyx.Erp.Application.IService.Products;
using Fancyx.Erp.Application.IService.Products.Models;
using Fancyx.Erp.EfCore.Entities;
using Fancyx.Shared.Exceptions;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.Service.Products
{
    internal class ProductCategoryService : IProductCategoryService
    {
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IMapper _mapper;

        public ProductCategoryService(IRepository<ProductCategory> productCategoryRepository, IMapper mapper)
        {
            _productCategoryRepository = productCategoryRepository;
            _mapper = mapper;
        }

        public async Task AddProductCategoryAsync(AddOrUpdateProductCategory req)
        {
            var codeIsExist = await _productCategoryRepository.AnyAsync(x => x.Code == req.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            var productCategory = new ProductCategory()
            {
                Code = req.Code,
                Name = req.Name,
                Remark = req.Remark,
                IsEnabled = req.IsEnabled
            };
            await _productCategoryRepository.InsertAsync(productCategory);
        }

        public async Task DeleteProductCategoryAsync(long id)
        {
            await _productCategoryRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<PagedResult<ProductCategoryItem>> GetProductCategoryListAsync(GetProductCategoryListRequest req)
        {
            var resp = await _productCategoryRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(req.Name), x => x.Name.StartsWith(req.Name!))
                .PagedAsync(req.Current, req.PageSize);
            return new PagedResult<ProductCategoryItem>(resp.Total, _mapper.Map<List<ProductCategoryItem>>(resp.Items));
        }

        public async Task UpdateProductCategoryAsync(AddOrUpdateProductCategory req)
        {
            var productCategory = await _productCategoryRepository.FindAsync(req.Id) ?? throw new EntityNotFoundException();
            var codeIsExist = productCategory.Code != req.Code && await _productCategoryRepository.AnyAsync(x => x.Code == req.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            productCategory.Code = req.Code;
            productCategory.Name = req.Name;
            productCategory.Remark = req.Remark;
            productCategory.IsEnabled = req.IsEnabled;
            await _productCategoryRepository.UpdateAsync(productCategory);
        }
    }
}