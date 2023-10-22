using AutoMapper;
using eShop.Accounts.Entities;
using eShop.Accounts.Exceptions;
using eShop.Accounts.Repositories;
using eShop.Messaging;

namespace eShop.Accounts.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IProducer _producer;

        public AccountService(IAccountRepository accountRepository, IMapper mapper, IProducer producer)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _producer = producer;
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
                result = await UpdateOrCreateAccountAsync(providerId, account);
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
                result = await UpdateOrCreateAccountAsync(providerId, account);
            }
            else
            {
                throw new AccountAlreadyRegisteredException();
            }

            return result;
        }

        private async Task<Account> UpdateOrCreateAccountAsync(Guid providerId, Account account)
        {
            Account result;

            var provider = await _accountRepository.GetAccountByIdAsync(providerId);
            if (provider != null)
            {
                var existingAccount = await _accountRepository.GetAccountByPhoneNumberAsync(account.PhoneNumber);
                if (existingAccount == null)
                {
                    result = account;

                    await _accountRepository.CreateAccountAsync(result);

                    var @event = new Messaging.Models.AccountRegisteredEvent
                    {
                        Account = _mapper.Map<Messaging.Models.Account>(result),
                        ProviderId = providerId,
                    };
                    _producer.Publish(@event);
                }
                else
                {
                    if (existingAccount.Id != providerId)
                    {
                        if (account.TelegramUserId.HasValue)
                        {
                            existingAccount.TelegramUserId = account.TelegramUserId;
                        }

                        if (account.ViberUserId.HasValue)
                        {
                            existingAccount.ViberUserId = account.ViberUserId;
                        }

                        if (account.IdentityUserId != null)
                        {
                            existingAccount.IdentityUserId = account.IdentityUserId;
                        }

                        await _accountRepository.UpdateAccountAsync(existingAccount);

                        var @event = new Messaging.Models.AccountUpdatedEvent
                        {
                            Account = _mapper.Map<Messaging.Models.Account>(existingAccount),
                        };
                        _producer.Publish(@event);

                        result = existingAccount;
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
