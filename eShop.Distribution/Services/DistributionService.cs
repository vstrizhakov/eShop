using eShop.Distribution.Entities;
using eShop.Distribution.Entities.History;
using eShop.Distribution.Exceptions;
using eShop.Distribution.Repositories;
using eShop.Messaging.Models;

namespace eShop.Distribution.Services
{
    public class DistributionService : IDistributionService
    {
        private readonly IDistributionRepository _distributionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IDistributionsHubServer _distributionHubServer;

        public DistributionService(
            IDistributionRepository distributionRepository,
            IAccountRepository accountRepository,
            IDistributionSettingsService distributionSettingsService,
            IDistributionsHubServer distributionHubServer)
        {
            _distributionRepository = distributionRepository;
            _accountRepository = accountRepository;
            _distributionSettingsService = distributionSettingsService;
            _distributionHubServer = distributionHubServer;
        }

        public async Task<Entities.Distribution> CreateDistributionAsync(Guid providerId, Announce composition)
        {
            var distribution = new Entities.Distribution
            {
                ProviderId = providerId,
            };

            var accounts = await _accountRepository.GetAccountsByProviderIdAsync(providerId, true, true);

            var shopId = composition.ShopId;
            foreach (var account in accounts)
            {
                var historyRecord = await CreateHistoryRecord(account.DistributionSettings);
                var shopSettings = historyRecord.ShopSettings;

                var telegramChatGroups = account.TelegramChats
                    .Where(e => e.IsEnabled)
                    .GroupBy(e => e.Account);
                foreach (var telegramChats in telegramChatGroups)
                {
                    var isFiltered = shopSettings.Filter && !shopSettings.PreferredShops.Any(e => e.Id == shopId);
                    foreach (var telegramChat in telegramChats)
                    {
                        var item = new DistributionItem
                        {
                            AccountId = account.Id,
                            TelegramChatId = telegramChat.Id,
                            DistributionSettings = historyRecord,
                        };

                        if (isFiltered)
                        {
                            item.Status = DistributionItemStatus.Filtered;
                        }

                        distribution.Items.Add(item);
                    }
                }

                var viberChats = new[] { account.ViberChat }
                    .Where(e => e != null && e.IsEnabled);
                foreach (var viberChat in viberChats)
                {
                    var item = new DistributionItem
                    {
                        AccountId = account.Id,
                        ViberChatId = viberChat.Id,
                        DistributionSettings = historyRecord,
                    };

                    var isFiltered = shopSettings.Filter && !shopSettings.PreferredShops.Any(e => e.Id == shopId);
                    if (isFiltered)
                    {
                        item.Status = DistributionItemStatus.Filtered;
                    }

                    distribution.Items.Add(item);
                }
            }

            await _distributionRepository.CreateDistributionAsync(distribution);

            return distribution;
        }

        public async Task<Entities.Distribution?> GetDistributionAsync(Guid distributionId)
        {
            var distribution = await _distributionRepository.GetDistributionByIdAsync(distributionId);
            return distribution;
        }

        public async Task UpdateDistributionRequestStatusAsync(Guid distributionRequestId, bool succeeded)
        {
            var request = await _distributionRepository.GetDistributionRequestAsync(distributionRequestId);
            if (request == null)
            {
                throw new DistributionRequestNotFoundException();
            }

            if (request.Status != DistributionItemStatus.Pending)
            {
                throw new InvalidDistributionRequestStatusException();
            }

            request.Status = succeeded ? DistributionItemStatus.Delivered : DistributionItemStatus.Failed;

            await _distributionRepository.UpdateDistributionItemAsync(request);

            await _distributionHubServer.SendRequestUpdatedAsync(request);
        }

        private async Task<DistributionSettingsRecord> CreateHistoryRecord(DistributionSettings distributionSettings)
        {
            var shopSettings = distributionSettings.ShopSettings;
            var shopSettingsRecord = new ShopSettingsRecord
            {
                Filter = shopSettings.Filter,
            };
            if (shopSettings.Filter)
            {
                shopSettingsRecord.PreferredShops = shopSettings.PreferredShops;
            }

            var comissionSettings = distributionSettings.ComissionSettings;
            var comissionSettingsRecord = new ComissionSettingsRecord
            {
                Amount = comissionSettings.Amount,
            };

            var preferredCurrency = distributionSettings.PreferredCurrency;
            var currencyRateRecords = Array.Empty<CurrencyRateRecord>();
            if (preferredCurrency != null)
            {
                var currencyRates = await _distributionSettingsService.GetCurrencyRatesAsync(distributionSettings);

                currencyRateRecords = currencyRates.Select(e => new CurrencyRateRecord
                {
                    CurrencyId = e.SourceCurrencyId,
                    Rate = e.Rate,
                }).ToArray();
            }
            var historyRecord = new DistributionSettingsRecord
            {
                PreferredCurrency = preferredCurrency,
                CurrencyRates = currencyRateRecords,
                ShowSales = distributionSettings.ShowSales,
                ShopSettings = shopSettingsRecord,
                ComissionSettings = comissionSettingsRecord,
            };

            return historyRecord;
        }
    }
}
