using eShop.Accounts.Entities;
using eShop.Accounts.Exceptions;
using eShop.Accounts.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Identity;
using eShop.Messaging.Models.Viber;

namespace eShop.Accounts.Handlers
{
    public class RegisterViberUserRequestHandler : IRequestHandler<RegisterViberUserRequest, RegisterViberUserResponse>
    {
        private readonly IAccountService _accountService;
        private readonly IProducer _producer;

        public RegisterViberUserRequestHandler(IAccountService accountService, IProducer producer)
        {
            _accountService = accountService;
            _producer = producer;
        }

        public async Task<RegisterViberUserResponse?> HandleRequestAsync(RegisterViberUserRequest request)
        {
            RegisterViberUserResponse? response = null;

            var phoneNumber = request.PhoneNumber;
            var viberUserId = request.ViberUserId;

            var account = await _accountService.GetAccountByPhoneNumberAsync(phoneNumber);
            if (account != null)
            {
                await _accountService.LinkViberUserAsync(account, viberUserId);

                response = new RegisterViberUserResponse
                {
                    IsSuccess = true,
                    AccountId = account.Id,
                    ViberUserId = viberUserId,
                    IsConfirmationRequested = request.IsConfirmationRequested,
                };
            }
            else
            {
                var getIdentityUserRequest = new GetIdentityUserRequest
                {
                    PhoneNumber = phoneNumber,
                    FirstName = request.Name,
                    ProviderId = request.ProviderId,
                    ViberUserId = viberUserId,
                    IsConfirmationRequested = request.IsConfirmationRequested,
                };
                _producer.Publish(getIdentityUserRequest);
            }

            return response;
        }
    }
}
