using eShop.Distribution.Entities;

namespace eShop.Distribution.Services
{
    public interface IAccountService
    {
        Task UpdateTelegramChatAsync(Guid accountId, Guid telegramChatId, bool isEnabled);
        Task UpdateViberChatAsync(Guid accountId, Guid viberChatId, bool isEnabled);
        Task CreateAccountAsync(Guid accountId, Guid? telegramUserId, Guid? viberUserId, string firstName, string lastName, Guid? announcerId);
        Task UpdateAccountAsync(Guid accountId, Guid? telegramUserId, Guid? viberUserId, string firstName, string lastName);
        Task<Account?> GetAccountByIdAsync(Guid accountId);
        Task<Account?> GetAccountByTelegramUserIdAsync(Guid accountId);
        Task<Account?> GetAccountByViberUserIdAsync(Guid accountId);
        Task SubscribeToAnnouncerAsync(Account account, Account announcer);
    }
}
