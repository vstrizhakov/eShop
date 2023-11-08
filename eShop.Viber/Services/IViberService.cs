using eShop.Viber.Entities;

namespace eShop.Viber.Services
{
    public interface IViberService
    {
        Task<ViberUser?> GetUserByIdAsync(string userId);
        Task<ViberUser?> GetUserByViberUserIdAsync(Guid viberUserId);
        Task UpdateUserAsync(ViberUser user);
        Task SetAccountIdAsync(ViberUser user, Guid accountId);
        Task SetIsChatEnabledAsync(ViberUser user, bool isEnabled);
        Task SetActiveContextAsync(ViberUser user, string? activeContext);
    }
}
