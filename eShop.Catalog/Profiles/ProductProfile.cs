using AutoMapper;
using eShop.Catalog.Entities;
using eShop.Catalog.Models.Products;

namespace eShop.Catalog.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductRequest, Product>()
                .ForMember(dest => dest.Prices, options =>
                    options.MapFrom(src => new List<Models.Products.ProductPrice>() { src.Price }))
                .ForMember(dest => dest.Images, options => options.Ignore())
                .ForMember(dest => dest.Id, options => options.Ignore())
                .ForMember(dest => dest.OwnerId, options => options.Ignore())
                .ForMember(dest => dest.CreatedAt, options => options.Ignore())
                .ForMember(dest => dest.Category, options => options.Ignore())
                .ForMember(dest => dest.Compositions, options => options.Ignore());
            CreateMap<Models.Products.ProductPrice, Entities.ProductPrice>()
                .ForMember(dest => dest.Value, options => options.MapFrom(src => src.Price))
                .ForMember(dest => dest.DiscountedValue, options => options.MapFrom(src => src.DiscountedPrice))
                .ForMember(dest => dest.Id, options => options.Ignore())
                .ForMember(dest => dest.ProductId, options => options.Ignore())
                .ForMember(dest => dest.CreatedAt, options => options.Ignore())
                .ForMember(dest => dest.Currency, options => options.Ignore())
                .ForMember(dest => dest.Product, options => options.Ignore());
        }
    }
}
