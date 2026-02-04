using AutoMapper;
using Cracker.EfCore;
using Fancyx.Erp.Application.IService.Products;
using Fancyx.Erp.Application.IService.Products.Models;
using Fancyx.Erp.EfCore.Entities;
using Fancyx.Erp.EfCore.Repositories;
using Fancyx.Shared.Exceptions;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.Service.Products
{
    public class ProductAttrService : IProductAttrService
    {
        private readonly IRepository<ProductAttr> _productAttrRepository;
        private readonly IRepository<ProductAttrValue> _productAttrValueRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<ProductBindAttrValue> _productBindAttrValueRepository;
        private readonly ProductRepository _productRepository;

        public ProductAttrService(IRepository<ProductAttr> productAttrRepository, IRepository<ProductAttrValue> productAttrValueRepository, IMapper mapper
            , IRepository<ProductBindAttrValue> productBindAttrValueRepository, ProductRepository productRepository)
        {
            _productAttrRepository = productAttrRepository;
            _productAttrValueRepository = productAttrValueRepository;
            _mapper = mapper;
            _productBindAttrValueRepository = productBindAttrValueRepository;
            _productRepository = productRepository;
        }

        public async Task AddProductAttrAsync(AddOrUpdateProductAttrRequest req)
        {
            var codeIsExist = await _productAttrRepository.AnyAsync(x => x.Code == req.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            var productAttr = _mapper.Map<ProductAttr>(req);
            await _productAttrRepository.InsertAsync(productAttr);
        }

        public async Task AddProductAttrValueAsync(AddOrUpdateProductAttrValueRequest req)
        {
            var codeIsExist = await _productAttrValueRepository.AnyAsync(x => x.Code == req.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            var productAttrValue = _mapper.Map<ProductAttrValue>(req);
            await _productAttrValueRepository.InsertAsync(productAttrValue);
        }

        public async Task DeleteProductAttrAsync(long id)
        {
            var isUsed = await _productBindAttrValueRepository.AnyAsync(x => x.AttrId == id);
            if (isUsed)
            {
                var productBindAttrValue = await _productBindAttrValueRepository.GetAsync(x => x.AttrValueId == id);
                var productName = (await _productRepository.GetProductSlimInfoAsync(productBindAttrValue!.ProductId))?.Name;
                throw new BusinessException($"属性已被商品【{productName}】使用，无法删除");
            }
            await _productAttrRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task DeleteProductAttrValueAsync(long id)
        {
            var isUsed = await _productBindAttrValueRepository.AnyAsync(x => x.AttrValueId == id);
            if (isUsed)
            {
                var productBindAttrValue = await _productBindAttrValueRepository.GetAsync(x => x.AttrValueId == id);
                var productName = (await _productRepository.GetProductSlimInfoAsync(productBindAttrValue!.ProductId))?.Name;
                throw new BusinessException($"属性值数据已被商品【{productName}】使用，无法删除");
            }
            await _productBindAttrValueRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<PagedResult<ProductAttrItem>> GetProductAttrListAsync(GetProductAttrListRequest req)
        {
            var data = await _productAttrRepository.GetQueryable().WhereIf(!string.IsNullOrEmpty(req.Name), x => x.Name.StartsWith(req.Name!))
                .PagedAsync(req.Current, req.PageSize);
            return new PagedResult<ProductAttrItem>(data.Total, _mapper.Map<List<ProductAttrItem>>(data.Items));
        }

        public async Task<PagedResult<ProductAttrValueItem>> GetProductAttrValueListAsync(GetProductAttrValueListRequest req)
        {
            var data = await _productAttrRepository.GetQueryable().WhereIf(!string.IsNullOrEmpty(req.Code), x => x.Name.StartsWith(req.Code!))
                .PagedAsync(req.Current, req.PageSize);
            return new PagedResult<ProductAttrValueItem>(data.Total, _mapper.Map<List<ProductAttrValueItem>>(data.Items));
        }

        public async Task UpdateProductAttrAsync(AddOrUpdateProductAttrRequest req)
        {
            var productAttr = await _productAttrRepository.FindAsync(req.Id) ?? throw new EntityNotFoundException();
            var codeIsExist = productAttr.Code != req.Code && await _productAttrRepository.AnyAsync(x => x.Code == req.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            _mapper.Map(req, productAttr);
            await _productAttrRepository.UpdateAsync(productAttr);
        }

        public async Task UpdateProductAttrValueAsync(AddOrUpdateProductAttrValueRequest req)
        {
            var productAttrValue = await _productAttrValueRepository.FindAsync(req.Id) ?? throw new EntityNotFoundException();
            var codeIsExist = productAttrValue.Code != req.Code && await _productAttrValueRepository.AnyAsync(x => x.Code == req.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            _mapper.Map(req, productAttrValue);
            await _productAttrValueRepository.UpdateAsync(productAttrValue);
        }
    }
}