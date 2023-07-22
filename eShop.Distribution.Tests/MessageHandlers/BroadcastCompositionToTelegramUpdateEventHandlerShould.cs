using eShop.Distribution.Exceptions;
using eShop.Distribution.MessageHandlers;
using eShop.Distribution.Services;
using eShop.Messaging.Models;

namespace eShop.Distribution.Tests.MessageHandlers
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
                RequestId = Guid.NewGuid(),
                Succeeded = succeeded,
            };

            var distributionService = new Mock<IDistributionService>();
            distributionService
                .Setup(e => e.UpdateDistributionRequestStatusAsync(message.RequestId, message.Succeeded))
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
                RequestId = Guid.NewGuid(),
            };

            var distributionService = new Mock<IDistributionService>();
            distributionService
                .Setup(e => e.UpdateDistributionRequestStatusAsync(message.RequestId, message.Succeeded))
                .ThrowsAsync(new DistributionRequestNotFoundException());

            var sut = new BroadcastMessageUpdateEventHandler(distributionService.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            distributionService.VerifyAll();
        }
    }
}
