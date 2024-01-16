using eShopping.Accounts.Exceptions;
using eShopping.Accounts.Entities;
using eShopping.Accounts.Repositories;

namespace eShopping.Accounts.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Account?> GetAccountByPhoneNumberAsync(string phoneNumber)
        {
            if (phoneNumber == null)
            {
                throw new ArgumentNullException(nameof(phoneNumber));
            }

            var account = await _accountRepository.GetAccountByPhoneNumberAsync(phoneNumber);
            return account;
        }

        public async Task<Account> RegisterAccountAsync(string phoneNumber, Account account)
        {
            if (phoneNumber == null)
            {
                throw new ArgumentNullException(nameof(phoneNumber));
            }

            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            var result = await _accountRepository.GetAccountByPhoneNumberAsync(phoneNumber);
            if (result != null)
            {
                throw new AccountAlreadyRegisteredException();
            }

            await _accountRepository.CreateAccountAsync(account);

            result = account;

            return result;
        }

        public async Task LinkTelegramUserAsync(Account account, Guid telegramUserId)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            if (account.TelegramUserId.HasValue)
            {
                throw new InvalidOperationException(); // TODO: make new exception type
                // TODO: probably need to skip if ids are the same
            }

            account.TelegramUserId = telegramUserId;

            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task LinkViberUserAsync(Account account, Guid viberUserId)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            if (account.ViberUserId.HasValue)
            {
                throw new InvalidOperationException(); // TODO: make new exception type
            }

            account.ViberUserId = viberUserId;

            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task LinkIdentityUserAsync(Account account, Guid identityUserId)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            if (identityUserId == null)
            {
                throw new ArgumentNullException(nameof(identityUserId));
            }

            if (account.IdentityUserId != null)
            {
                throw new InvalidOperationException(); // TODO: make new exception type
            }

            account.IdentityUserId = identityUserId;

            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task<Account?> GetAccountByIdAsync(Guid id)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);
            return account;
        }
    }
}
