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

        public async Task CreateAccountAsync(Guid accountId, Guid? telegramUserId, Guid? viberUserId, string firstName, string lastName, Guid? announcerId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account != null)
            {
                throw new AccountAlreadyExistsException();
            }

            account = new Account
            {
                Id = accountId,
                TelegramUserId = telegramUserId,
                ViberUserId = viberUserId,
                FirstName = firstName,
                LastName = lastName,
                AnnouncerId = announcerId,
                IsActivated = true,
                DistributionSettings = new DistributionSettings
                {
                    AccountId = accountId,
                    ComissionSettings = new ComissionSettings(),
                    ShopSettings = new ShopSettings(),
                },
            };

            await _accountRepository.CreateAccountAsync(account);
        }

        public async Task UpdateAccountAsync(Guid accountId, Guid? telegramUserId, Guid? viberUserId, string firstName, string lastName)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null)
            {
                throw new InvalidOperationException(); // TODO: custom error
            }

            account.TelegramUserId = telegramUserId;
            account.ViberUserId = viberUserId;
            account.FirstName = firstName;
            account.LastName = lastName;

            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task SubscribeToAnnouncerAsync(Account account, Account announcer)
        {
            account.AnnouncerId = announcer.Id;
            account.IsActivated = true;

            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task<Account?> GetAccountByIdAsync(Guid accountId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            return account;
        }

        public async Task<Account?> GetAccountByTelegramUserIdAsync(Guid accountId)
        {
            var account = await _accountRepository.GetAccountByTelegramUserIdAsync(accountId);
            return account;
        }

        public async Task<Account?> GetAccountByViberUserIdAsync(Guid accountId)
        {
            var account = await _accountRepository.GetViberByTelegramUserIdAsync(accountId);
            return account;
        }
    }
}
