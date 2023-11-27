using eShop.Distribution.Entities;
using eShop.Distribution.MessageHandlers;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Contracts;

namespace eShop.Distribution.Tests.MessageHandlers
{
    public class BroadcastCompositionMessageHandlerShould
    {
        [Fact]
        public async Task HandleMessage()
        {
            // Arrange

            var composition = new Announce
            {
                Id = Guid.NewGuid(),
            };
            var message = new BroadcastAnnounceMessage
            {
                AnnouncerId = Guid.NewGuid(),
                Announce = composition,
            };

            BroadcastAnnounceUpdateEvent? updateEvent = null;
            BroadcastCompositionToTelegramMessage? broadcastToTelegramMessage = null;
            BroadcastCompositionToViberMessage? broadcastToViberMessage = null;

            var producer = new Mock<IProducer>();
            producer
                .Setup(e => e.Publish(It.IsAny<BroadcastAnnounceUpdateEvent>()))
                .Callback<BroadcastAnnounceUpdateEvent>(e => updateEvent = e);
            producer
                .Setup(e => e.Publish(It.IsAny<BroadcastCompositionToTelegramMessage>()))
                .Callback<BroadcastCompositionToTelegramMessage>(e => broadcastToTelegramMessage = e);
            producer
                .Setup(e => e.Publish(It.IsAny<BroadcastCompositionToViberMessage>()))
                .Callback<BroadcastCompositionToViberMessage>(e => broadcastToViberMessage = e);

            var telegramRequest = new DistributionItem
            {
                TelegramChatId = Guid.NewGuid(),
                DistributionSettings = new DistributionSettings(),
            };
            var viberRequest = new DistributionItem
            {
                ViberChatId = Guid.NewGuid(),
                DistributionSettings = new DistributionSettings(),
            };
            var distributionSettings = new DistributionSettings();
            var distribution = new Entities.Distribution()
            {
                Items = new List<DistributionItem>
                {
                    telegramRequest,
                    viberRequest,
                },
            };
            var distributionService = new Mock<IDistributionService>();
            distributionService
                .Setup(e => e.CreateDistributionFromProviderIdAsync(message.AnnouncerId))
                .ReturnsAsync(distribution);

            var messageFromComposition = new Message();
            var messageBuilder = new Mock<IMessageBuilder>();
            messageBuilder
                .Setup(e => e.FromComposition(composition, telegramRequest.DistributionSettings))
                .Returns(messageFromComposition);
            messageBuilder
                .Setup(e => e.FromComposition(composition, viberRequest.DistributionSettings))
                .Returns(messageFromComposition);

            var sut = new BroadcastCompositionMessageHandler(producer.Object, distributionService.Object, messageBuilder.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            producer.VerifyAll();
            distributionService.VerifyAll();
            messageBuilder.VerifyAll();

            Assert.NotNull(updateEvent);
            Assert.Equal(composition.Id, updateEvent.AnnounceId);
            Assert.Equal(distribution.Id, updateEvent.DistributionId);

            Assert.NotNull(broadcastToTelegramMessage);
            Assert.Equal(messageFromComposition, broadcastToTelegramMessage.Message);
            Assert.Equal(telegramRequest.TelegramChatId, broadcastToTelegramMessage.TargetId);
            Assert.Equal(telegramRequest.Id, broadcastToTelegramMessage.RequestId);

            Assert.NotNull(broadcastToViberMessage);
            Assert.Equal(messageFromComposition, broadcastToViberMessage.Message);
            Assert.Equal(viberRequest.ViberChatId, broadcastToViberMessage.TargetId);
            Assert.Equal(viberRequest.Id, broadcastToViberMessage.RequestId);
        }
    }
}
