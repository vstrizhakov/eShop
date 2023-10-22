namespace eShop.Distribution.Services
{
    public interface IAccountService
    {
        Task UpdateTelegramChatAsync(Guid accountId, Guid telegramChatId, bool isEnabled);
        Task UpdateViberChatAsync(Guid accountId, Guid viberChatId, bool isEnabled);
        Task CreateAccountAsync(Guid accountId, string firstName, string lastName, Guid providerId);
        Task UpdateAccountAsync(Guid accountId, string firstName, string lastName);
    }
}
