using eShop.Messaging.Contracts.Distribution.ShopSettings;

namespace eShop.Distribution.Profiles
{
    public class ShopSettingsProfile : AutoMapper.Profile
    {
        public ShopSettingsProfile()
        {
            CreateMap<Entities.ShopSettings, ShopSettings>();
            CreateMap<Aggregates.ShopFilter, Shop>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Shop.Id))
                .ForMember(dest => dest.Name, options => options.MapFrom(src => src.Shop.Name));
        }
    }
}
