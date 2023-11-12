using eShop.Accounts.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Identity;
using eShop.Messaging.Models.Telegram;

namespace eShop.Accounts.Handlers
{
    public class RegisterTelegramUserRequestHandler : IRequestHandler<RegisterTelegramUserRequest, RegisterTelegramUserResponse>
    {
        private readonly IAccountService _accountService;
        private readonly IProducer _producer;

        public RegisterTelegramUserRequestHandler(IAccountService accountService, IProducer producer)
        {
            _accountService = accountService;
            _producer = producer;
        }

        public async Task<RegisterTelegramUserResponse?> HandleRequestAsync(RegisterTelegramUserRequest request)
        {
            RegisterTelegramUserResponse? response = null;

            var phoneNumber = request.PhoneNumber;
            var telegramUserId = request.TelegramUserId;

            var account = await _accountService.GetAccountByPhoneNumberAsync(phoneNumber);
            if (account != null)
            {
                await _accountService.LinkTelegramUserAsync(account, telegramUserId);

                response = new RegisterTelegramUserResponse
                {
                    TelegramUserId = telegramUserId,
                    AccountId = account.Id,
                };
            }
            else
            {
                var getIdentityUserRequest = new GetIdentityUserRequest
                {
                    PhoneNumber = phoneNumber,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    ProviderId = request.ProviderId,
                    TelegramUserId = telegramUserId,
                    IsConfirmationRequested = request.IsConfirmationRequested,
                };
                _producer.Publish(getIdentityUserRequest);
            }

            return response;
        }
    }
}
