using eShop.Accounts.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution.ResetPassword;

namespace eShop.Accounts.Handlers
{
    public class SendResetPasswordMessageHandler : IMessageHandler<SendResetPasswordMessage>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IProducer _producer;

        public SendResetPasswordMessageHandler(IAccountRepository accountRepository, IProducer producer)
        {
            _accountRepository = accountRepository;
            _producer = producer;
        }

        public async Task HandleMessageAsync(SendResetPasswordMessage message)
        {
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
                    _producer.Publish(telegramMessage);
                }

                var viberUserId = account.ViberUserId;
                if (viberUserId.HasValue)
                {
                    var viberMessage = new SendResetPasswordToViberMessage
                    {
                        TargetId = viberUserId.Value,
                        ResetPasswordLink = message.ResetPasswordLink,
                    };
                    _producer.Publish(viberMessage);
                }
            }
        }
    }
}
