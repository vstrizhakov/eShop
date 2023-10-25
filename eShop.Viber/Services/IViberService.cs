using eShop.Viber.Entities;

namespace eShop.Viber.Services
{
    public interface IViberService
    {
        Task<ViberUser?> GetUserByIdAsync(string userId);
        Task SetRegistrationProviderIdAsync(ViberUser user, Guid providerId);
        Task UpdateUserAsync(ViberUser user);
        Task SetAccountIdAsync(ViberUser user, Guid accountId);
        Task SetIsChatEnabledAsync(ViberUser user, bool isEnabled);
        Task SetActiveContextAsync(ViberUser user, string? activeContext);
    }
}
