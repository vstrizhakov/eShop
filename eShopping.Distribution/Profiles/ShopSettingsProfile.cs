using eShopping.Distribution.Aggregates;
using eShopping.Messaging.Contracts.Distribution.ShopSettings;

namespace eShopping.Distribution.Profiles
{
    public class ShopSettingsProfile : AutoMapper.Profile
    {
        public ShopSettingsProfile()
        {
            CreateMap<Entities.ShopSettings, ShopSettings>();
            CreateMap<ShopFilter, Shop>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Shop.Id))
                .ForMember(dest => dest.Name, options => options.MapFrom(src => src.Shop.Name));
        }
    }
}
