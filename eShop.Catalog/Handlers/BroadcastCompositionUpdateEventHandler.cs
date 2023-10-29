using eShop.Catalog.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Catalog.Handlers
{
    public class BroadcastCompositionUpdateEventHandler : IMessageHandler<BroadcastCompositionUpdateEvent>
    {
        private readonly ICompositionRepository _compositionRepository;

        public BroadcastCompositionUpdateEventHandler(ICompositionRepository compositionRepository)
        {
            _compositionRepository = compositionRepository;
        }

        public async Task HandleMessageAsync(BroadcastCompositionUpdateEvent message)
        {
            var composition = await _compositionRepository.GetCompositionByIdAsync(message.CompositionId);
            if (composition != null)
            {
                composition.DistributionGroupId = message.DistributionGroupId;

                await _compositionRepository.UpdateCompositionAsync(composition);
            }
        }
    }
}
