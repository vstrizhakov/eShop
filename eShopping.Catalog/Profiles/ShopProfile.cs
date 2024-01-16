using AutoMapper;

namespace eShopping.Catalog.Profiles
{
    public class ShopProfile : Profile
    {
        public ShopProfile()
        {
            CreateMap<Entities.EmbeddedShop, Models.Shops.Shop>();
            CreateMap<Entities.Shop, Models.Shops.Shop>();
            CreateMap<Entities.Shop, Messaging.Contracts.Catalog.Shop>();
        }
    }
}
