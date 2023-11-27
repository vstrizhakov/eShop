using eShop.Accounts.Services;
using eShop.Messaging.Contracts.Viber;
using eShop.Messaging.Contracts.Identity;
using MassTransit;
using eShop.Messaging.Contracts.Distribution;

namespace eShop.Accounts.Consumers
{
    public class RegisterViberUserRequestHandler : IConsumer<RegisterViberUserRequest>
    {
        private readonly IAccountService _accountService;

        public RegisterViberUserRequestHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task Consume(ConsumeContext<RegisterViberUserRequest> context)
        {
            var request = context.Message;

            var phoneNumber = request.PhoneNumber;
            var viberUserId = request.ViberUserId;

            var account = await _accountService.GetAccountByPhoneNumberAsync(phoneNumber);
            if (account != null)
            {
                await _accountService.LinkViberUserAsync(account, viberUserId);

                var response = new RegisterViberUserResponse
                {
                    IsSuccess = true,
                    AccountId = account.Id,
                    ViberUserId = viberUserId,
                    IsConfirmationRequested = request.IsConfirmationRequested,
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
                    FirstName = request.Name,
                    AnnouncerId = request.AnnouncerId,
                    ViberUserId = viberUserId,
                    IsConfirmationRequested = request.IsConfirmationRequested,
                };

                await context.Publish(getIdentityUserRequest);
            }
        }
    }
}
