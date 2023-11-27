using eShop.Accounts.Services;
using eShop.Messaging.Contracts.Telegram;
using eShop.Messaging.Contracts.Identity;
using MassTransit;
using eShop.Messaging.Contracts.Distribution;

namespace eShop.Accounts.Consumers
{
    public class RegisterTelegramUserRequestHandler : IConsumer<RegisterTelegramUserRequest>
    {
        private readonly IAccountService _accountService;

        public RegisterTelegramUserRequestHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task Consume(ConsumeContext<RegisterTelegramUserRequest> context)
        {
            var request = context.Message;

            var phoneNumber = request.PhoneNumber;
            var telegramUserId = request.TelegramUserId;

            var account = await _accountService.GetAccountByPhoneNumberAsync(phoneNumber);
            if (account != null)
            {
                await _accountService.LinkTelegramUserAsync(account, telegramUserId);

                var response = new RegisterTelegramUserResponse
                {
                    TelegramUserId = telegramUserId,
                    AccountId = account.Id,
                };

                await context.Publish(response);

                var announcerId = request.AnnouncerId;
                if (announcerId.HasValue)
                {
                    var command = new SubscribeToAnnouncerRequest
                    {
                        AccountId = account.Id,
                        AnnouncerId = announcerId.Value,
                    };

                    await context.Publish(command);
                }
            }
            else
            {
                var getIdentityUserRequest = new GetIdentityUserRequest
                {
                    PhoneNumber = phoneNumber,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    AnnouncerId = request.AnnouncerId,
                    TelegramUserId = telegramUserId,
                    IsConfirmationRequested = request.IsConfirmationRequested,
                };

                await context.Publish(getIdentityUserRequest);
            }
        }
    }
}
