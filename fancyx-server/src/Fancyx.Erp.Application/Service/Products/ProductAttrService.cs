using AutoMapper;
using Fancyx.EfCore;
using Fancyx.Erp.Application.IService.Products;
using Fancyx.Erp.Application.IService.Products.Dtos;
using Fancyx.Erp.EfCore.Entites;
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

        public async Task AddProductAttrAsync(ProductAttrDto dto)
        {
            var codeIsExist = await _productAttrRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            var productAttr = _mapper.Map<ProductAttr>(dto);
            await _productAttrRepository.InsertAsync(productAttr);
        }

        public async Task AddProductAttrValueAsync(ProductAttrValueDto dto)
        {
            var codeIsExist = await _productAttrValueRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            var productAttrValue = _mapper.Map<ProductAttrValue>(dto);
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

        public async Task<PagedResult<ProductAttrListDto>> GetProductAttrListAsync(ProductAttrQueryDto dto)
        {
            var data = await _productAttrRepository.GetQueryable().WhereIf(!string.IsNullOrEmpty(dto.Name), x => x.Name.StartsWith(dto.Name!))
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<ProductAttrListDto>(data.Total, _mapper.Map<List<ProductAttrListDto>>(data.Items));
        }

        public async Task<PagedResult<ProductAttrValueListDto>> GetProductAttrValueListAsync(ProductAttrValueQueryDto dto)
        {
            var data = await _productAttrRepository.GetQueryable().WhereIf(!string.IsNullOrEmpty(dto.Code), x => x.Name.StartsWith(dto.Code!))
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<ProductAttrValueListDto>(data.Total, _mapper.Map<List<ProductAttrValueListDto>>(data.Items));
        }

        public async Task UpdateProductAttrAsync(ProductAttrDto dto)
        {
            var productAttr = await _productAttrRepository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();
            var codeIsExist = productAttr.Code != dto.Code && await _productAttrRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            _mapper.Map(dto, productAttr);
            await _productAttrRepository.UpdateAsync(productAttr);
        }

        public async Task UpdateProductAttrValueAsync(ProductAttrValueDto dto)
        {
            var productAttrValue = await _productAttrValueRepository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();
            var codeIsExist = productAttrValue.Code != dto.Code && await _productAttrValueRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            _mapper.Map(dto, productAttrValue);
            await _productAttrValueRepository.UpdateAsync(productAttrValue);
        }
    }
}