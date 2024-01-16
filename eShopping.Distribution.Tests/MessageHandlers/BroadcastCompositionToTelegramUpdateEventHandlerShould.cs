using eShopping.Distribution.Exceptions;
using eShopping.Distribution.MessageHandlers;
using eShopping.Distribution.Services;
using eShopping.Messaging.Contracts;

namespace eShopping.Distribution.Tests.MessageHandlers
{
    public class BroadcastCompositionToTelegramUpdateEventHandlerShould
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Publish(bool succeeded)
        {
            // Arrange

            var message = new BroadcastMessageUpdateEvent
            {
                DistributionItemId = Guid.NewGuid(),
                Succeeded = succeeded,
            };

            var distributionService = new Mock<IDistributionService>();
            distributionService
                .Setup(e => e.SetDistributionItemStatusAsync(message.DistributionItemId, message.Succeeded))
                .Returns(Task.CompletedTask);

            var sut = new BroadcastMessageUpdateEventHandler(distributionService.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            distributionService.VerifyAll();
        }

        [Fact]
        public async Task HandleDistributionRequestNotFoundException()
        {
            // Arrange

            var message = new BroadcastMessageUpdateEvent
            {
                DistributionItemId = Guid.NewGuid(),
            };

            var distributionService = new Mock<IDistributionService>();
            distributionService
                .Setup(e => e.SetDistributionItemStatusAsync(message.DistributionItemId, message.Succeeded))
                .ThrowsAsync(new DistributionRequestNotFoundException());

            var sut = new BroadcastMessageUpdateEventHandler(distributionService.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            distributionService.VerifyAll();
        }
    }
}
