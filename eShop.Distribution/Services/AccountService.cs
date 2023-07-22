using eShop.Distribution.Entities;
using eShop.Distribution.Exceptions;
using eShop.Distribution.Repositories;

namespace eShop.Distribution.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task UpdateTelegramChatAsync(Guid accountId, Guid telegramChatId, bool isEnabled)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null)
            {
                throw new AccountNotFoundException();
            }

            await _accountRepository.UpdateTelegramChatAsync(account, telegramChatId, isEnabled);
        }

        public async Task UpdateViberChatAsync(Guid accountId, Guid viberChatId, bool isEnabled)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null)
            {
                throw new AccountNotFoundException();
            }

            await _accountRepository.UpdateViberChatAsync(account, viberChatId, isEnabled);
        }

        public async Task CreateNewAccountAsync(Guid accountId, Guid providerId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account != null)
            {
                throw new AccountAlreadyExistsException();
            }

            account = new Account
            {
                Id = accountId,
                ProviderId = providerId,
            };

            await _accountRepository.CreateAccountAsync(account);
        }
    }
}
