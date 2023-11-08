using eShop.Accounts.Entities;
using eShop.Accounts.Exceptions;
using eShop.Accounts.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Telegram;

namespace eShop.Accounts.Handlers
{
    public class RegisterTelegramUserRequestHandler : IRequestHandler<RegisterTelegramUserRequest, RegisterTelegramUserResponse>
    {
        private readonly IAccountService _accountService;

        public RegisterTelegramUserRequestHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<RegisterTelegramUserResponse> HandleRequestAsync(RegisterTelegramUserRequest request)
        {
            try
            {
                var providerId = request.ProviderId;
                var accountInfo = new Account
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    TelegramUserId = request.TelegramUserId,
                };

                var account = await _accountService.RegisterAccountByTelegramUserIdAsync(providerId, accountInfo);

                var response = new RegisterTelegramUserResponse
                {
                    AccountId = account.Id,
                    TelegramUserId = account.TelegramUserId!.Value,
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
