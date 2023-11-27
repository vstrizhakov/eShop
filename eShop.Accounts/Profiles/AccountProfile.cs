using eShop.Messaging.Contracts;

namespace eShop.Accounts.Profiles
{
    public class AccountProfile : AutoMapper.Profile
    {
        public AccountProfile()
        {
            CreateMap<Entities.Account, Account>();
        }
    }
}
