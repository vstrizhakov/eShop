using eShop.Distribution.Entities;
using eShop.Distribution.Repositories;

namespace eShop.Distribution.Services
{
    public class DistributionSettingsService : IDistributionSettingsService
    {
        private readonly IDistributionSettingsRepository _distributionSettingsRepository;

        public DistributionSettingsService(IDistributionSettingsRepository distributionSettingsRepository)
        {
            _distributionSettingsRepository = distributionSettingsRepository;
        }

        public async Task<DistributionSettings> GetDistributionSettingsAsync(Guid accountId)
        {
            var distributionSettings = await _distributionSettingsRepository.GetActiveDistributionSettingsAsync(accountId);
            if (distributionSettings == null)
            {
                distributionSettings = new DistributionSettings();
            }

            return distributionSettings;
        }

        public async Task<DistributionSettings> UpdateDistributionSettingsAsync(Guid accountId, DistributionSettings distributionSettings)
        {
            var oldDistributionSettings = await _distributionSettingsRepository.GetActiveDistributionSettingsAsync(accountId);
            if (oldDistributionSettings != null)
            {
                if (distributionSettings.PreferredCurrencyId == default)
                {
                    distributionSettings.PreferredCurrencyId = oldDistributionSettings.PreferredCurrencyId;
                }
            }

            distributionSettings.AccountId = accountId;
            await _distributionSettingsRepository.CreateDistributionSettingsAsync(distributionSettings);

            return distributionSettings;
        }
    }
}
