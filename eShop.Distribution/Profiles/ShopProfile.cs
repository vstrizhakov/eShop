using eShop.Messaging.Contracts.Catalog;

namespace eShop.Distribution.Profiles
{
    public class ShopProfile : AutoMapper.Profile
    {
        public ShopProfile()
        {
            CreateMap<Shop, Entities.Shop>();
        }
    }
}
