using eShop.Identity.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Identity.Handlers
{
    public class AccountRegisteredEventHandler : IMessageHandler<AccountRegisteredEvent>
    {
        private readonly IUserRepository _userRepository;

        public AccountRegisteredEventHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleMessageAsync(AccountRegisteredEvent message)
        {
            var user = await _userRepository.GetUserByIdAsync(message.Account.IdentityUserId);
            if (user != null)
            {
                user.AccountId = message.Account.Id;

                await _userRepository.UpdateUserAsync(user);
            }
        }
    }
}
