using eShop.Distribution.Entities;
using eShop.Distribution.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Distribution.MessageHandlers
{
    public class BroadcastCompositionToViberUpdateEventHandler : IMessageHandler<BroadcastCompositionToViberUpdateEvent>
    {
        private readonly IDistributionRepository _distributionRepository;

        public BroadcastCompositionToViberUpdateEventHandler(IDistributionRepository distributionRepository)
        {
            _distributionRepository = distributionRepository;
        }

        public async Task HandleMessageAsync(BroadcastCompositionToViberUpdateEvent message)
        {
            var distributionItem = await _distributionRepository.GetDistributionGroupItemByViberChatIdAsync(message.DistributionGroupId, message.ViberChatId);
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
