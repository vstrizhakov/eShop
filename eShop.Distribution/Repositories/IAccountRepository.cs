using eShop.Distribution.Entities;

namespace eShop.Distribution.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountByIdAsync(Guid id, Guid? providerId = null);
        Task<IEnumerable<Account>> GetAccountsByProviderIdAsync(Guid providerId, bool? isActivated = null, bool includeDistributionSettings = false);
        Task CreateAccountAsync(Account account);
        Task UpdateTelegramChatAsync(Account account, Guid telegramChatId, bool isEnabled);
        Task UpdateViberChatAsync(Account account, Guid viberUserId, bool isEnabled);
        Task UpdateIsActivatedAsync(Account clientAccount, bool isActivated);
    }
}
