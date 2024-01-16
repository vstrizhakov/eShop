namespace eShopping.Distribution.Profiles
{
    public class ShopProfile : AutoMapper.Profile
    {
        public ShopProfile()
        {
            CreateMap<Messaging.Contracts.Catalog.Shop, Entities.Shop>();
        }
    }
}
