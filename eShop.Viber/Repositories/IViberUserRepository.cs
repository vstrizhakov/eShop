using eShop.Viber.Entities;

namespace eShop.Viber.Repositories
{
    public interface IViberUserRepository
    {
        Task CreateViberUserAsync(ViberUser viberUser);
        Task<ViberUser?> GetViberUserByExternalIdAsync(string externalId);
        Task<ViberUser?> GetViberUserByIdAsync(Guid id);
        Task<ViberUser?> GetViberUserByAccountIdAsync(Guid accountId);
        Task<IEnumerable<ViberUser>> GetViberUsersByIdsAsync(IEnumerable<Guid> ids);
        Task UpdateAccountIdAsync(ViberUser viberUser, Guid accountId);
        Task UpdateChatSettingsAsync(ViberUser viberUser, bool isEnabled);
        Task UpdateViberUserAsync(ViberUser viberUser);
    }
}
