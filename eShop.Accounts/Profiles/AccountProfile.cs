using eShop.Messaging.Contracts;
using eShop.Messaging.Contracts.Distribution;

namespace eShop.Accounts.Profiles
{
    public class AccountProfile : AutoMapper.Profile
    {
        public AccountProfile()
        {
            CreateMap<Entities.Account, Account>();
            CreateMap<Entities.Account, Announcer>();
        }
    }
}
