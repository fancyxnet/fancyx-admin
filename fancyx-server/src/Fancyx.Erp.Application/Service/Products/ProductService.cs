using AutoMapper;
using Fancyx.EfCore;
using Fancyx.EfCore.Aop;
using Fancyx.Erp.Application.IService.Products;
using Fancyx.Erp.Application.IService.Products.Dtos;
using Fancyx.Erp.EfCore.Entites;
using Fancyx.Erp.EfCore.Repositories;
using Fancyx.Shared.Exceptions;
using Fancyx.Shared.Models;
using Fancyx.SnowflakeId;

namespace Fancyx.Erp.Application.Service.Products
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<ProductBindAttrValue> _productBindAttrValueRepository;
        private readonly IRepository<Inventory> _inventoryRepository;

        public ProductService(ProductRepository productRepository, IMapper mapper, IRepository<ProductBindAttrValue> productBindAttrValueRepository
            , IRepository<Inventory> inventoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _productBindAttrValueRepository = productBindAttrValueRepository;
            _inventoryRepository = inventoryRepository;
        }

        [AsyncTransactional]
        public async Task AddProductAsync(ProductDto dto)
        {
            var codeIsExist = await _productRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编号已存在");
            }
            var product = _mapper.Map<Product>(dto);
            product.Id = IdGenerater.Instance.NextId();
            await _productRepository.InsertAsync(product);
            if (dto.Attrs?.Count > 0)
            {
                var productBindAttrValues = dto.Attrs.Select(x => new ProductBindAttrValue
                {
                    ProductId = product.Id,
                    AttrId = x.AttrId,
                    AttrValue = x.AttrValue,
                    AttrValueId = x.AttrValueId,
                }).ToList();
                await _productBindAttrValueRepository.InsertManyAsync(productBindAttrValues);
            }
        }

        [AsyncTransactional]
        public async Task DeleteProductAsync(long id)
        {
            var hasStock = await _inventoryRepository.AnyAsync(x => x.ProductId == id);
            if (hasStock)
            {
                throw new BusinessException("商品存在库存记录，无法删除");
            }
            await _productBindAttrValueRepository.DeleteAsync(x => x.ProductId == id);
            await _productRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<PagedResult<ProductListDto>> GetProductListAsync(ProductQueryDto dto)
        {
            var data = await _productRepository.QueryProductListAsync(dto.Current, dto.PageSize, dto.Name);
            return new PagedResult<ProductListDto>(data.Total, _mapper.Map<List<ProductListDto>>(data.Items));
        }

        [AsyncTransactional]
        public async Task UpdateProductAsync(ProductUpdateDto dto)
        {
            var product = await _productRepository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();
            var codeIsExist = product.Code != dto.Code && await _productRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编号已存在");
            }
            _mapper.Map(dto, product);
            await _productRepository.UpdateAsync(product);
            if (dto.Attrs?.Count > 0)
            {
                await _productBindAttrValueRepository.DeleteAsync(x => x.ProductId == product.Id);
                var productBindAttrValues = dto.Attrs.Select(x => new ProductBindAttrValue
                {
                    ProductId = product.Id,
                    AttrId = x.AttrId,
                    AttrValue = x.AttrValue,
                    AttrValueId = x.AttrValueId,
                }).ToList();
                await _productBindAttrValueRepository.InsertManyAsync(productBindAttrValues);
            }
        }
    }
}