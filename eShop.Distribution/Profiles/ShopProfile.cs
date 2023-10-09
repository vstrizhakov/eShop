namespace eShop.Distribution.Profiles
{
    public class ShopProfile : AutoMapper.Profile
    {
        public ShopProfile()
        {
            CreateMap<Messaging.Models.Catalog.Shop, Entities.Shop>();
        }
    }
}
