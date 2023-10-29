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

        public DistributionService(IDistributionRepository distributionRepository, IAccountRepository accountRepository, IDistributionSettingsService distributionSettingsService)
        {
            _distributionRepository = distributionRepository;
            _accountRepository = accountRepository;
            _distributionSettingsService = distributionSettingsService;
        }

        public async Task<DistributionGroup> CreateDistributionAsync(Guid providerId, Composition composition)
        {
            var distributionGroup = new DistributionGroup
            {
                ProviderId = providerId,
            };

            var accounts = await _accountRepository.GetAccountsByProviderIdAsync(providerId, true, true);

            var shopId = composition.ShopId;

            var telegramChatGroups = accounts
                .SelectMany(e => e.TelegramChats)
                .Where(e => e.IsEnabled)
                .GroupBy(e => e.Account);
            foreach (var telegramChats in telegramChatGroups)
            {
                var historyRecord = await CreateHistoryRecord(telegramChats.Key.DistributionSettings);
                var shopSettings = historyRecord.ShopSettings;

                var isFiltered = shopSettings.Filter && !shopSettings.PreferredShops.Any(e => e.Id == shopId);
                foreach (var telegramChat in telegramChats)
                {
                    var distributionGroupItem = new DistributionGroupItem
                    {
                        TelegramChatId = telegramChat.Id,
                        DistributionSettings = historyRecord,
                    };

                    if (isFiltered)
                    {
                        distributionGroupItem.Status = DistributionGroupItemStatus.Filtered;
                    }

                    distributionGroup.Items.Add(distributionGroupItem);
                }
            }

            var viberChats = accounts
                .Where(e => e.ViberChat != null && e.ViberChat.IsEnabled)
                .Select(e => e.ViberChat);
            foreach (var viberChat in viberChats)
            {
                var historyRecord = await CreateHistoryRecord(viberChat.Account.DistributionSettings);
                var shopSettings = historyRecord.ShopSettings;

                var distributionGroupItem = new DistributionGroupItem
                {
                    ViberChatId = viberChat.Id,
                    DistributionSettings = historyRecord,
                };

                var isFiltered = shopSettings.Filter && !shopSettings.PreferredShops.Any(e => e.Id == shopId);
                if (isFiltered)
                {
                    distributionGroupItem.Status = DistributionGroupItemStatus.Filtered;
                }

                distributionGroup.Items.Add(distributionGroupItem);
            }

            await _distributionRepository.CreateDistributionGroupAsync(distributionGroup);

            return distributionGroup;
        }

        public async Task UpdateDistributionRequestStatusAsync(Guid distributionRequestId, bool deliveryFailed)
        {
            var request = await _distributionRepository.GetDistributionRequestAsync(distributionRequestId);
            if (request == null)
            {
                throw new DistributionRequestNotFoundException();
            }

            if (request.Status != DistributionGroupItemStatus.Pending)
            {
                throw new InvalidDistributionRequestStatusException();
            }

            request.Status = deliveryFailed ? DistributionGroupItemStatus.Failed : DistributionGroupItemStatus.Delivered;

            await _distributionRepository.UpdateDistributionGroupItemAsync(request);
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
