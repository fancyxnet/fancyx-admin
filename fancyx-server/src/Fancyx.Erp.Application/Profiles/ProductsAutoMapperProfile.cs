using AutoMapper;
using Fancyx.Erp.Application.IService.Products.Models;
using Fancyx.Erp.EfCore.Entities;
using Fancyx.Erp.EfCore.Models;

namespace Fancyx.Erp.Application.Profiles
{
    public class ProductsAutoMapperProfile : Profile
    {
        public ProductsAutoMapperProfile()
        {
            CreateMap<ProductCategoryItem, ProductCategory>(MemberList.None);
            CreateMap<AddOrUpdateProductRequest, Product>(MemberList.None);
            CreateMap<UpdateProductRequest, Product>(MemberList.None);
            CreateMap<AddOrUpdateProductAttrRequest, ProductAttr>(MemberList.None);
            CreateMap<AddOrUpdateProductAttrValueRequest, ProductAttrValue>(MemberList.None);
            CreateMap<ProductAttr, ProductAttrItem>(MemberList.None);
            CreateMap<ProductAttrValue, ProductAttrValueItem>(MemberList.None);
            CreateMap<AddOrUpdateProductBrand, ProductBrand>(MemberList.None);
            CreateMap<ProductBrand, ProductBrandItem>(MemberList.None);
        }
    }
}