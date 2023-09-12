using eShop.Distribution.Entities;
using eShop.Distribution.Exceptions;
using eShop.Distribution.Repositories;

namespace eShop.Distribution.Services
{
    public class DistributionService : IDistributionService
    {
        private readonly IDistributionRepository _distributionRepository;
        private readonly IAccountRepository _accountRepository;

        public DistributionService(IDistributionRepository distributionRepository, IAccountRepository accountRepository)
        {
            _distributionRepository = distributionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<DistributionGroup> CreateDistributionFromProviderIdAsync(Guid providerId)
        {
            var distributionGroup = new DistributionGroup
            {
                ProviderId = providerId,
            };

            var accounts = await _accountRepository.GetAccountsByProviderIdAsync(providerId, true);

            var telegramChatIds = accounts
                .SelectMany(e => e.TelegramChats)
                .Where(e => e.IsEnabled)
                .Select(e => e.Id)
                .Distinct();
            foreach (var telegramChatId in telegramChatIds)
            {
                var distributionGroupItem = new DistributionGroupItem
                {
                    TelegramChatId = telegramChatId,
                };

                distributionGroup.Items.Add(distributionGroupItem);
            }

            var viberChatIds = accounts
                .Where(e => e.ViberChat != null)
                .Select(e => e.ViberChat)
                .Where(e => e.IsEnabled)
                .Select(e => e.Id)
                .Distinct();
            foreach (var viberChatId in viberChatIds)
            {
                var distributionGroupItem = new DistributionGroupItem
                {
                    ViberChatId = viberChatId,
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
    }
}
