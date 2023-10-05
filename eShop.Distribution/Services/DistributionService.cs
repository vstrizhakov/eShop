using eShop.Distribution.Entities;
using eShop.Distribution.Exceptions;
using eShop.Distribution.Repositories;

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

        public async Task<DistributionGroup> CreateDistributionFromProviderIdAsync(Guid providerId)
        {
            var distributionGroup = new DistributionGroup
            {
                ProviderId = providerId,
            };

            var accounts = await _accountRepository.GetAccountsByProviderIdAsync(providerId, true, true);

            var telegramChatGroups = accounts
                .SelectMany(e => e.TelegramChats)
                .Where(e => e.IsEnabled)
                .GroupBy(e => e.Account);
            foreach (var telegramChats in telegramChatGroups)
            {
                var historyRecord = await CreateHistoryRecord(telegramChats.Key.DistributionSettings);

                foreach (var telegramChat in telegramChats)
                {
                    var distributionGroupItem = new DistributionGroupItem
                    {
                        TelegramChatId = telegramChat.Id,
                        DistributionSettings = historyRecord,
                    };

                    distributionGroup.Items.Add(distributionGroupItem);
                }
            }

            var viberChats = accounts
                .Where(e => e.ViberChat != null)
                .Select(e => e.ViberChat)
                .Where(e => e.IsEnabled);
            foreach (var viberChat in viberChats)
            {
                var historyRecord = await CreateHistoryRecord(viberChat.Account.DistributionSettings);

                var distributionGroupItem = new DistributionGroupItem
                {
                    ViberChatId = viberChat.Id,
                    DistributionSettings = historyRecord,
                };

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

        private async Task<DistributionSettingsHistoryRecord> CreateHistoryRecord(DistributionSettings distributionSettings)
        {
            // TODO: Check preferre currency on null
            var currencyRates = await _distributionSettingsService.GetCurrencyRatesAsync(distributionSettings);

            var historyRecord = new DistributionSettingsHistoryRecord
            {
                PreferredCurrency = distributionSettings.PreferredCurrency,
                CurrencyRates = currencyRates.Select(e => new CurrencyRateHistoryRecord
                {
                    CurrencyId = e.SourceCurrencyId,
                    Rate = e.Rate,
                }).ToList(),
            };

            return historyRecord;
        }
    }
}
