using eShopping.Messaging.Contracts.Distribution.ResetPassword;
using eShopping.Accounts.Repositories;
using eShopping.Messaging.Contracts.Distribution.ResetPassword;
using MassTransit;

namespace eShopping.Accounts.Consumers
{
    public class SendResetPasswordMessageHandler : IConsumer<SendResetPasswordMessage>
    {
        private readonly IAccountRepository _accountRepository;

        public SendResetPasswordMessageHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task Consume(ConsumeContext<SendResetPasswordMessage> context)
        {
            var message = context.Message;
            var account = await _accountRepository.GetAccountByIdAsync(message.AccountId);
            if (account != null)
            {
                var telegramUserId = account.TelegramUserId;
                if (telegramUserId.HasValue)
                {
                    var telegramMessage = new SendResetPasswordToTelegramMessage
                    {
                        TargetId = telegramUserId.Value,
                        ResetPasswordLink = message.ResetPasswordLink,
                    };

                    await context.Publish(telegramMessage);
                }

                var viberUserId = account.ViberUserId;
                if (viberUserId.HasValue)
                {
                    var viberMessage = new SendResetPasswordToViberMessage
                    {
                        TargetId = viberUserId.Value,
                        ResetPasswordLink = message.ResetPasswordLink,
                    };

                    await context.Publish(viberMessage);
                }
            }
        }
    }
}
