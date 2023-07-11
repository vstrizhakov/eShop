using AutoMapper;
using eShop.Distribution.Entities;
using eShop.Distribution.Models;

namespace eShop.Distribution.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Account, Client>();
            CreateMap<TelegramChat, Chat>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id));
            CreateMap<ViberChat, Chat>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id));
        }
    }
}
