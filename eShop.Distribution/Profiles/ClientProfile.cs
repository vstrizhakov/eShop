using AutoMapper;

namespace eShop.Distribution.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Entities.Account, Models.Client>();
            CreateMap<Entities.TelegramChat, Models.Chat>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id));
            CreateMap<Entities.ViberChat, Models.Chat>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id));
        }
    }
}
