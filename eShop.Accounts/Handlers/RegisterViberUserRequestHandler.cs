using eShop.Accounts.Entities;
using eShop.Accounts.Exceptions;
using eShop.Accounts.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Viber;

namespace eShop.Accounts.Handlers
{
    public class RegisterViberUserRequestHandler : IRequestHandler<RegisterViberUserRequest, RegisterViberUserResponse>
    {
        private readonly IAccountService _accountService;

        public RegisterViberUserRequestHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<RegisterViberUserResponse> HandleRequestAsync(RegisterViberUserRequest message)
        {
            try
            {
                var providerId = message.ProviderId;
                var accountInfo = new Account
                {
                    FirstName = message.Name,
                    PhoneNumber = message.PhoneNumber,
                    ViberUserId = message.ViberUserId,
                };

                var account = await _accountService.RegisterAccountByViberUserIdAsync(providerId, accountInfo);

                var response = new RegisterViberUserResponse
                {
                    IsSuccess = true,
                    AccountId = account.Id,
                    ViberUserId = account.ViberUserId,
                };

                return response;
            }
            catch (AccountAlreadyRegisteredException)
            {
                // Publish message with error
            }
            catch (ProviderNotExistsException)
            {
                // Publish message with error
            }

            return new RegisterViberUserResponse
            {
                IsSuccess = false, // TODO: add error description
            };
        }
    }
}
