using eShop.Distribution.Entities;
using eShop.Distribution.MessageHandlers;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Distribution.Tests.MessageHandlers
{
    public class BroadcastCompositionMessageHandlerShould
    {
        [Fact]
        public async Task HandleMessage()
        {
            // Arrange

            var composition = new Composition
            {
                Id = Guid.NewGuid(),
            };
            var message = new BroadcastCompositionMessage
            {
                ProviderId = Guid.NewGuid(),
                Composition = composition,
            };

            BroadcastCompositionUpdateEvent? updateEvent = null;
            BroadcastCompositionToTelegramMessage? broadcastToTelegramMessage = null;
            BroadcastCompositionToViberMessage? broadcastToViberMessage = null;

            var producer = new Mock<IProducer>();
            producer
                .Setup(e => e.Publish(It.IsAny<BroadcastCompositionUpdateEvent>()))
                .Callback<BroadcastCompositionUpdateEvent>(e => updateEvent = e);
            producer
                .Setup(e => e.Publish(It.IsAny<BroadcastCompositionToTelegramMessage>()))
                .Callback<BroadcastCompositionToTelegramMessage>(e => broadcastToTelegramMessage = e);
            producer
                .Setup(e => e.Publish(It.IsAny<BroadcastCompositionToViberMessage>()))
                .Callback<BroadcastCompositionToViberMessage>(e => broadcastToViberMessage = e);

            var distribution = new DistributionGroup()
            {
                Items = new List<DistributionGroupItem>
                {
                    new DistributionGroupItem
                    {
                        TelegramChatId = Guid.NewGuid(),
                    },
                    new DistributionGroupItem
                    {
                        ViberChatId = Guid.NewGuid(),
                    },
                },
            };
            var distributionService = new Mock<IDistributionService>();
            distributionService
                .Setup(e => e.CreateDistributionFromProviderIdAsync(message.ProviderId))
                .ReturnsAsync(distribution);

            var messageFromComposition = new Message();
            var messageBuilder = new Mock<IMessageBuilder>();
            messageBuilder
                .Setup(e => e.FromComposition(composition))
                .Returns(messageFromComposition);

            var sut = new BroadcastCompositionMessageHandler(producer.Object, distributionService.Object, messageBuilder.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            producer.VerifyAll();
            distributionService.VerifyAll();
            messageBuilder.VerifyAll();

            Assert.NotNull(updateEvent);
            Assert.Equal(composition.Id, updateEvent.CompositionId);
            Assert.Equal(distribution.Id, updateEvent.DistributionGroupId);

            Assert.NotNull(broadcastToTelegramMessage);
            Assert.Equal(messageFromComposition, broadcastToTelegramMessage.Message);
            Assert.Equal(distribution.Items.Where(e => e.TelegramChatId.HasValue).Count(), broadcastToTelegramMessage.Requests.Count());

            Assert.NotNull(broadcastToViberMessage);
            Assert.Equal(messageFromComposition, broadcastToViberMessage.Message);
            Assert.Equal(distribution.Items.Where(e => e.ViberChatId.HasValue).Count(), broadcastToViberMessage.Requests.Count());
        }
    }
}
