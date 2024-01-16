using eShopping.Accounts.Entities;
using eShopping.Messaging.Contracts.Distribution;

namespace eShopping.Accounts.Profiles
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
