using eShop.Distribution.Entities;

namespace eShop.Distribution.Services
{
    public interface IAccountService
    {
        Task UpdateTelegramChatAsync(Guid accountId, Guid telegramChatId, bool isEnabled);
        Task UpdateViberChatAsync(Guid accountId, Guid viberChatId, bool isEnabled);
        Task CreateAccountAsync(Guid accountId, string firstName, string lastName, Guid? announcerId);
        Task UpdateAccountAsync(Guid accountId, string firstName, string lastName, Guid? announcerId);
        Task<Account?> GetAccountAsync(Guid accountId);
        Task SubscribeToAnnouncerAsync(Account account, Account announcer);
    }
}
