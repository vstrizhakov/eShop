using eShop.Distribution.Aggregates;
using eShop.Distribution.Entities;
using eShop.Distribution.Repositories;

namespace eShop.Distribution.Services
{
    public class DistributionSettingsService : IDistributionSettingsService
    {
        private readonly IDistributionSettingsRepository _distributionSettingsRepository;
        private readonly IShopRepository _shopRepository;

        public DistributionSettingsService(IDistributionSettingsRepository distributionSettingsRepository, IShopRepository shopRepository)
        {
            _distributionSettingsRepository = distributionSettingsRepository;
            _shopRepository = shopRepository;
        }

        public async Task<DistributionSettings?> GetDistributionSettingsAsync(Guid accountId)
        {
            var distributionSettings = await _distributionSettingsRepository.GetDistributionSettingsAsync(accountId);
            return distributionSettings;
        }

        public async Task<DistributionSettings> SetPreferredCurrencyAsync(DistributionSettings distributionSettings, Guid currencyId)
        {
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

        public async Task<DistributionSettings> SetCurrencyRateAsync(DistributionSettings distributionSettings, Guid sourceCurrencyId, double rate)
        {
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

        public async Task<DistributionSettings> SetComissionAmountAsync(DistributionSettings distributionSettings, decimal amount)
        {
            distributionSettings.ComissionSettings.Amount = amount;

            await _distributionSettingsRepository.UpdateDistributionSettingsAsync(distributionSettings);

            return distributionSettings;
        }

        public async Task<DistributionSettings> SetFilterShopsAsync(DistributionSettings distributionSettings, bool filter)
        {
            distributionSettings.ShopSettings.Filter = filter;

            await _distributionSettingsRepository.UpdateDistributionSettingsAsync(distributionSettings);

            return distributionSettings;
        }

        public async Task<IEnumerable<ShopFilter>> GetShopsAsync(DistributionSettings distributionSettings)
        {
            if (!distributionSettings.ShopSettings.Filter)
            {
                throw new InvalidOperationException();
            }

            var shops = await _shopRepository.GetShopsAsync();

            var filterShops = distributionSettings.ShopSettings.PreferredShops.Select(shop => new ShopFilter(shop, true));
            filterShops = filterShops.Concat(shops.Select(shop => new ShopFilter(shop, false)));
            filterShops = filterShops.DistinctBy(e => e.Shop.Id);
            filterShops = filterShops.OrderBy(e => e.Shop.Name);

            return filterShops;
        }

        public async Task<DistributionSettings> SetShopIsEnabledAsync(DistributionSettings distributionSettings, Guid shopId, bool isEnabled)
        {
            var shops = distributionSettings.ShopSettings.PreferredShops;
            var shop = shops.FirstOrDefault(e => e.Id == shopId);
            if (isEnabled)
            {
                if (shop == null)
                {
                    shop = await _shopRepository.GetShopAsync(shopId);
                    shops.Add(shop!);
                }
            }
            else
            {
                if (shop != null)
                {
                    shops.Remove(shop);
                }
            }

            await _distributionSettingsRepository.UpdateDistributionSettingsAsync(distributionSettings);

            return distributionSettings;
        }
    }
}
