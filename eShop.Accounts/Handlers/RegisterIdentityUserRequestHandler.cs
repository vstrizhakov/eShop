using eShop.Accounts.Entities;
using eShop.Accounts.Exceptions;
using eShop.Accounts.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Identity;

namespace eShop.Accounts.Handlers
{
    public class RegisterIdentityUserRequestHandler : IRequestHandler<RegisterIdentityUserRequest, RegisterIdentityUserResponse>
    {
        private readonly IAccountService _accountService;

        public RegisterIdentityUserRequestHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<RegisterIdentityUserResponse> HandleRequestAsync(RegisterIdentityUserRequest request)
        {
            try
            {
                var accountInfo = new Account
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    IdentityUserId = request.IdentityUserId,
                    TelegramUserId = request.TelegramUserId,
                    ViberUserId = request.ViberUserId,
                };

                var account = await _accountService.RegisterAccountByIdentityUserIdAsync(accountInfo);

                var response = new RegisterIdentityUserResponse
                {
                    IdentityUserId = request.IdentityUserId,
                    TelegramUserId = request.TelegramUserId,
                    ViberUserId = request.ViberUserId,
                    AccountId = account.Id,
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

            return null; // TODO: handle
        }
    }
}
