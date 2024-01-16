using eShopping.Messaging.Contracts.Distribution;

namespace eShopping.Distribution.Profiles
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
