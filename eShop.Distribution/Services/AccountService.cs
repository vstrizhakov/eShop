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

        public async Task CreateAccountAsync(Guid accountId, string firstName, string lastName, Guid? providerId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account != null)
            {
                throw new AccountAlreadyExistsException();
            }

            account = new Account
            {
                Id = accountId,
                FirstName = firstName,
                LastName = lastName,
                ProviderId = providerId,
                DistributionSettings = new DistributionSettings
                {
                    AccountId = accountId,
                    ComissionSettings = new ComissionSettings(),
                    ShopSettings = new ShopSettings(),
                },
            };

            await _accountRepository.CreateAccountAsync(account);
        }

        public async Task UpdateAccountAsync(Guid accountId, string firstName, string lastName, Guid? providerId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null)
            {
                throw new InvalidOperationException(); // TODO: custom error
            }

            if (providerId.HasValue) // TODO: handle this case some another way
            {
                account.ProviderId = providerId;
            }
            account.FirstName = firstName;
            account.LastName = lastName;

            await _accountRepository.UpdateAccountAsync(account);
        }
    }
}
