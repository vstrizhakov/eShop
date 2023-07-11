using eShop.Distribution.Entities;
using eShop.Distribution.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Distribution.MessageHandlers
{
    public class BroadcastCompositionToTelegramUpdateEventHandler : IMessageHandler<BroadcastCompositionToTelegramUpdateEvent>
    {
        private readonly IDistributionRepository _distributionRepository;

        public BroadcastCompositionToTelegramUpdateEventHandler(IDistributionRepository distributionRepository)
        {
            _distributionRepository = distributionRepository;
        }

        public async Task HandleMessageAsync(BroadcastCompositionToTelegramUpdateEvent message)
        {
            var distributionItem = await _distributionRepository.GetDistributionGroupItemByTelegramChatIdAsync(message.DistributionGroupId, message.TelegramChatId);
            if (distributionItem != null)
            {
                distributionItem.Status = message.Succeeded ? DistributionGroupItemStatus.Delivered : DistributionGroupItemStatus.Failed;

                await _distributionRepository.UpdateDistributionGroupItemAsync(distributionItem);
            }
            else
            {
                // TODO: Handle
            }
        }
    }
}
