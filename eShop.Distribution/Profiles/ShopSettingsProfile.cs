namespace eShop.Distribution.Profiles
{
    public class ShopSettingsProfile : AutoMapper.Profile
    {
        public ShopSettingsProfile()
        {
            CreateMap<Entities.ShopSettings, Messaging.Models.Distribution.ShopSettings.ShopSettings>();
            CreateMap<Aggregates.ShopFilter, Messaging.Models.Distribution.ShopSettings.Shop>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Shop.Id))
                .ForMember(dest => dest.Name, options => options.MapFrom(src => src.Shop.Name));
        }
    }
}
