namespace eShop.Distribution.Services
{
    public interface IAccountService
    {
        Task UpdateTelegramChatAsync(Guid accountId, Guid telegramChatId, bool isEnabled);
        Task UpdateViberChatAsync(Guid accountId, Guid viberChatId, bool isEnabled);
        Task CreateNewAccountAsync(Guid accountId, Guid providerId);
    }
}
