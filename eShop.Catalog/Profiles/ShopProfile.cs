using AutoMapper;

namespace eShop.Catalog.Profiles
{
    public class ShopProfile : Profile
    {
        public ShopProfile()
        {
            CreateMap<Entities.Shop, Models.Shops.Shop>();
        }
    }
}
