using eShopping.Identity.Repositories;
using eShopping.Messaging.Contracts;
using MassTransit;

namespace eShopping.Identity.Consumers
{
    public class AccountRegisteredEventConsumer : IConsumer<AccountRegisteredEvent>
    {
        private readonly IUserRepository _userRepository;

        public AccountRegisteredEventConsumer(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<AccountRegisteredEvent> context)
        {
            var message = context.Message;
            var identityUserId = message.Account.IdentityUserId;
            if (identityUserId != null)
            {
                var user = await _userRepository.GetUserByIdAsync(identityUserId.Value);
                if (user != null)
                {
                    user.AccountId = message.Account.Id;

                    await _userRepository.UpdateUserAsync(user);
                }
            }
        }
    }
}
