using eShop.Messaging.Contracts.Distribution;

namespace eShop.Distribution.Profiles
{
    public class AccountProfile : AutoMapper.Profile
    {
        public AccountProfile()
        {
            CreateMap<Entities.Account, Models.Account>();
            CreateMap<Entities.Account, Announcer>();
        }
    }
}
