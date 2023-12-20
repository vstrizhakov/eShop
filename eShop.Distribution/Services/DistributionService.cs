using eShop.Distribution.Entities;
using eShop.Distribution.Entities.History;
using eShop.Distribution.Exceptions;
using eShop.Distribution.Repositories;
using eShop.Messaging.Contracts;

namespace eShop.Distribution.Services
{
    public class DistributionService : IDistributionService
    {
        private readonly IDistributionRepository _distributionRepository;
        private readonly IAccountService _accountService;
        private readonly IDistributionsHubServer _distributionHubServer;

        public DistributionService(
            IDistributionRepository distributionRepository,
            IAccountService accountService,
            IDistributionsHubServer distributionHubServer)
        {
            _distributionRepository = distributionRepository;
            _accountService = accountService;
            _distributionHubServer = distributionHubServer;
        }

        public async Task<Entities.Distribution> CreateDistributionAsync(Guid announcerId, Announce composition)
        {
            var distribution = new Entities.Distribution
            {
                AnnouncerId = announcerId,
            };

            var accounts = await _accountService.GetSubscribersAsync(announcerId);

            var shopId = composition.ShopId;
            foreach (var account in accounts)
            {
                var historyRecord = await CreateHistoryRecord(account);
                var shopSettings = historyRecord.ShopSettings;
                var isFiltered = shopSettings.Filter && !shopSettings.PreferredShops.Any(shop => shop.Id == shopId);

                var group = new DistributionGroup
                {
                    Account = account.GeneratedEmbedded(),
                    DistributionSettings = historyRecord,
                };

                var itemStatus = DistributionItemStatus.Pending;
                if (isFiltered)
                {
                    itemStatus = DistributionItemStatus.Filtered;
                }

                var telegramChats = account.TelegramChats
                    .Where(e => e.IsEnabled);
                foreach (var telegramChat in telegramChats)
                {
                    var item = new DistributionItem
                    {
                        TelegramChatId = telegramChat.Id,
                        Status = itemStatus,
                    };

                    group.Items.Add(item);
                }

                var viberChats = new[] { account.ViberChat }
                    .Where(e => e != null && e.IsEnabled);
                foreach (var viberChat in viberChats)
                {
                    var item = new DistributionItem
                    {
                        ViberChatId = viberChat.Id,
                        Status = itemStatus,
                    };

                    group.Items.Add(item);
                }

                distribution.Targets.Add(group);
            }

            await _distributionRepository.CreateDistributionAsync(distribution);

            return distribution;
        }

        public async Task<Entities.Distribution?> GetDistributionAsync(Guid distributionId, Guid announcerId)
        {
            var distribution = await _distributionRepository.GetDistributionAsync(distributionId, announcerId);
            return distribution;
        }

        public async Task SetDistributionItemStatusAsync(Guid distributionId, Guid announcerId, Guid distributionItemId, bool succeeded)
        {
            var distribution = await _distributionRepository.GetDistributionAsync(distributionId, announcerId);
            if (distribution == null)
            {
                throw new DistributionRequestNotFoundException();
            }

            var distributionItem = distribution.Targets.SelectMany(e => e.Items).FirstOrDefault(e => e.Id == distributionItemId);
            if (distributionItem == null)
            {
                throw new DistributionRequestNotFoundException();
            }

            if (distributionItem.Status != DistributionItemStatus.Pending)
            {
                throw new InvalidDistributionRequestStatusException();
            }

            distributionItem.Status = succeeded ? DistributionItemStatus.Delivered : DistributionItemStatus.Failed;

            await _distributionRepository.UpdateDistributionAsync(distribution);

            await _distributionHubServer.SendRequestUpdatedAsync(distributionId, distributionItem);
        }

        private async Task<DistributionSettingsRecord> CreateHistoryRecord(Entities.Account account)
        {
            var distributionSettings = account.DistributionSettings;
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
                var currencyRates = await _accountService.GetCurrencyRatesAsync(account);

                currencyRateRecords = currencyRates.Select(e => new CurrencyRateRecord
                {
                    Currency = e.SourceCurrency,
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
