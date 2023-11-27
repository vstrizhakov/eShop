using eShop.Catalog.MessageHandlers;
using eShop.Catalog.Repositories;
using eShop.Messaging.Contracts;

namespace eShop.Catalog.Tests.MessageHandlers
{
    public class BroadcastCompositionUpdateEventHandlerShould
    {
        [Fact]
        public async Task UpdateComposition()
        {
            // Arrange

            var composition = new Entities.Announce();

            var message = new BroadcastAnnounceUpdateEvent
            {
                AnnounceId = composition.Id,
                DistributionId = Guid.NewGuid(),
            };

            var compositionRepository = new Mock<IAnnounceRepository>();
            compositionRepository
                .Setup(e => e.GetAnnounceByIdAsync(composition.Id))
                .ReturnsAsync(composition);

            compositionRepository
                .Setup(e => e.UpdateAnnounceAsync(It.IsAny<Entities.Announce>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act

            var messageHandler = new BroadcastCompositionUpdateEventHandler(compositionRepository.Object);

            await messageHandler.HandleMessageAsync(message);

            // Assert

            compositionRepository.VerifyAll();

            Assert.Equal(message.DistributionId, composition.DistributionId);
        }
    }
}
