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
            var distributionSettings = (await _distributionSettingsRepository.GetDistributionSettingsAsync(accountId))!;
            return distributionSettings;
        }

        public async Task<DistributionSettings> SetPreferredCurrencyAsync(Guid accountId, Guid currencyId)
        {
            var distributionSettings = (await _distributionSettingsRepository.GetDistributionSettingsAsync(accountId))!;

            distributionSettings.PreferredCurrencyId = currencyId;

            await _distributionSettingsRepository.UpdateDistributionSettingsAsync(distributionSettings);

            return distributionSettings;
        }

        public async Task<IEnumerable<CurrencyRate>> GetCurrencyRatesAsync(DistributionSettings distributionSettings)
        {
            if (distributionSettings.PreferredCurrencyId == null)
            {
                throw new InvalidOperationException(); // TODO: not sure we need this a per history record
            }

            var preferredCurrencyId = distributionSettings.PreferredCurrencyId.Value;
            var defaultCurrencyRates = await _distributionSettingsRepository.GetDefaultCurrencyRatesAsync(preferredCurrencyId);
            var customCurrencyRates = distributionSettings.CurrencyRates.Where(e => e.TargetCurrencyId == preferredCurrencyId);

            var currencyRates = customCurrencyRates
                .Concat(defaultCurrencyRates)
                .DistinctBy(e => e.SourceCurrencyId);
            return currencyRates;
        }

        public async Task<DistributionSettings> SetCurrencyRateAsync(Guid accountId, Guid sourceCurrencyId, double rate)
        {
            var distributionSettings = (await _distributionSettingsRepository.GetDistributionSettingsAsync(accountId))!;
            if (distributionSettings.PreferredCurrencyId == null)
            {
                throw new InvalidOperationException(); // TODO: not sure we need this a per history record
            }

            var targetCurrencyId = distributionSettings.PreferredCurrencyId.Value;
            var currencyRates = distributionSettings.CurrencyRates;

            var currencyRate = currencyRates.FirstOrDefault(e => e.TargetCurrencyId == targetCurrencyId && e.SourceCurrencyId == sourceCurrencyId);
            if (currencyRate == null)
            {
                var defaultCurrencyRates = await _distributionSettingsRepository.GetDefaultCurrencyRatesAsync(targetCurrencyId);
                if (!defaultCurrencyRates.Any(e => e.SourceCurrencyId == sourceCurrencyId))
                {
                    throw new InvalidOperationException();
                }

                currencyRate = new CurrencyRate
                {
                    TargetCurrencyId = targetCurrencyId,
                    SourceCurrencyId = sourceCurrencyId,
                };

                currencyRates.Add(currencyRate);
            }

            currencyRate.Rate = rate;

            await _distributionSettingsRepository.UpdateDistributionSettingsAsync(distributionSettings);

            return distributionSettings;
        }
    }
}
