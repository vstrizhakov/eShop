using eShop.Catalog.MessageHandlers;
using eShop.Catalog.Repositories;
using eShop.Messaging.Models;
using Moq;

namespace eShop.Catalog.Tests.MessageHandlers
{
    public class BroadcastCompositionUpdateEventHandlerShould
    {
        [Fact]
        public async Task UpdateComposition()
        {
            // Arrange

            var composition = new Entities.Composition();

            var message = new BroadcastCompositionUpdateEvent
            {
                CompositionId = composition.Id,
                DistributionGroupId = Guid.NewGuid(),
            };

            var compositionRepository = new Mock<ICompositionRepository>();
            compositionRepository
                .Setup(e => e.GetCompositionByIdAsync(composition.Id))
                .ReturnsAsync(composition);

            compositionRepository
                .Setup(e => e.UpdateCompositionAsync(It.IsAny<Entities.Composition>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act

            var messageHandler = new BroadcastCompositionUpdateEventHandler(compositionRepository.Object);

            await messageHandler.HandleMessageAsync(message);

            // Assert

            compositionRepository.VerifyAll();

            Assert.Equal(message.DistributionGroupId, composition.DistributionGroupId);
        }
    }
}
