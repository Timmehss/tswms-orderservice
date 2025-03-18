using AutoMapper;
using TSWMS.ProductService.Api.Dto;
using TSWMS.ProductService.Shared.Models;

namespace TSWMS.ProductService.Api.MappingProfiles;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>();
    }
}
