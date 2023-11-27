using eShop.Identity.Repositories;
using eShop.Messaging.Contracts;
using MassTransit;

namespace eShop.Identity.Consumers
{
    public class AccountRegisteredEventHandler : IConsumer<AccountRegisteredEvent>
    {
        private readonly IUserRepository _userRepository;

        public AccountRegisteredEventHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<AccountRegisteredEvent> context)
        {
            var message = context.Message;
            var identityUserId = message.Account.IdentityUserId;
            if (identityUserId != null)
            {
                var user = await _userRepository.GetUserByIdAsync(identityUserId);
                if (user != null)
                {
                    user.AccountId = message.Account.Id;

                    await _userRepository.UpdateUserAsync(user);
                }
            }
        }
    }
}
