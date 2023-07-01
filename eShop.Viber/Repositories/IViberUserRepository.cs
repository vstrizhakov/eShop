using eShop.Database.Data;

namespace eShop.Viber.Repositories
{
    public interface IViberUserRepository
    {
        Task CreateViberUserAsync(ViberUser viberUser);
        Task<ViberUser?> GetViberUserByExternalIdAsync(string externalId);
        Task<ViberUser?> GetViberUserByIdAsync(Guid viberUserId);
        Task UpdateAccountIdAsync(ViberUser viberUser, Guid accountId);
        Task UpdateChatSettingsAsync(ViberUser viberUser, bool isEnabled);
        Task UpdateViberUserAsync(ViberUser viberUser);
    }
}
