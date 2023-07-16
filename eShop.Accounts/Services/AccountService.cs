using eShop.Accounts.Entities;
using eShop.Accounts.Exceptions;
using eShop.Accounts.Repositories;

namespace eShop.Accounts.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Account> RegisterAccountByTelegramUserIdAsync(Guid providerId, Account account)
        {
            if (!account.TelegramUserId.HasValue)
            {
                throw new ArgumentException(nameof(account.TelegramUserId));
            }

            var telegramUserId = account.TelegramUserId.Value;

            var result = await _accountRepository.GetAccountByTelegramUserIdAsync(telegramUserId);
            if (result == null)
            {
                result = await GetOrCreateAccountAsync(providerId, account);
            }
            else
            {
                throw new AccountAlreadyRegisteredException();
            }

            return result;
        }

        public async Task<Account> RegisterAccountByViberUserIdAsync(Guid providerId, Account account)
        {
            if (!account.ViberUserId.HasValue)
            {
                throw new ArgumentException(nameof(account.ViberUserId));
            }

            var viberUserId = account.ViberUserId.Value;

            var result = await _accountRepository.GetAccountByViberUserIdAsync(viberUserId);
            if (result == null)
            {
                result = await GetOrCreateAccountAsync(providerId, account);
            }
            else
            {
                throw new AccountAlreadyRegisteredException();
            }

            return result;
        }

        private async Task<Account> GetOrCreateAccountAsync(Guid providerId, Account account)
        {
            Account result;

            var provider = await _accountRepository.GetAccountByIdAsync(providerId);
            if (provider != null)
            {
                result = await _accountRepository.GetAccountByPhoneNumberAsync(account.PhoneNumber);
                if (result == null)
                {
                    result = account;
                 
                    await _accountRepository.CreateAccountAsync(result);
                }
                else
                {
                    if (result.Id != providerId)
                    {
                        result.TelegramUserId = account.TelegramUserId;
                        result.ViberUserId = account.ViberUserId;
                        result.IdentityUserId = account.IdentityUserId;

                        await _accountRepository.UpdateAccountAsync(result);
                    }
                    else
                    {
                        throw new InvalidProviderException();
                    }
                }
            }
            else
            {
                throw new ProviderNotExistsException();
            }

            return result;
        }
    }
}
