using AutoMapper;

namespace eShopping.Distribution.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Entities.Account, Models.Client>();
            CreateMap<Entities.EmbeddedAccount, Models.Client>();
            CreateMap<Entities.TelegramChat, Models.Chat>();
            CreateMap<Entities.ViberChat, Models.Chat>();
        }
    }
}
