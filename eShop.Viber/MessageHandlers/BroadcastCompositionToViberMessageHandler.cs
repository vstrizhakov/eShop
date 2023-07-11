using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Extensions;
using eShop.Messaging.Models;
using eShop.RabbitMq;
using eShop.Viber.Models;
using eShop.Viber.Repositories;
using eShop.ViberBot;

namespace eShop.Viber.MessageHandlers
{
    public class BroadcastCompositionToViberMessageHandler : IMessageHandler<BroadcastCompositionToViberMessage>
    {
        private readonly IViberBotClient _botClient;
        private readonly IViberUserRepository _viberUserRepository;
        private readonly IBotContextConverter _botContextConverter;
        private readonly IRabbitMqProducer _producer;

        public BroadcastCompositionToViberMessageHandler(
            IViberBotClient botClient,
            IViberUserRepository viberUserRepository,
            IBotContextConverter botContextConverter,
            IRabbitMqProducer producer)
        {
            _botClient = botClient;
            _viberUserRepository = viberUserRepository;
            _botContextConverter = botContextConverter;
            _producer = producer;
        }

        public async Task HandleMessageAsync(BroadcastCompositionToViberMessage message)
        {
            var viberUsers = await _viberUserRepository.GetViberUsersByIdsAsync(message.ViberChatIds);
            foreach (var viberUser in viberUsers)
            {
                var succeeded = true;
                try
                {
                    var sender = new User
                    {
                        Name = "Test",
                    };
                    var keyboard = new Keyboard
                    {
                        Buttons = new[]
                        {
                        new Button
                        {
                            Rows = 1,
                            Text = "Налаштування анонсів",
                            ActionBody = _botContextConverter.Serialize(ViberContext.Settings),
                        },
                    },
                    };
                    await _botClient.SendPictureMessageAsync(viberUser.ExternalId, sender, message.Image.ToString(), message.Caption, keyboard: keyboard);
                }
                catch
                {
                    succeeded = false;
                }

                var update = new BroadcastCompositionToViberUpdateEvent
                {
                    DistributionGroupId = message.DistributionGroupId,
                    ViberChatId = viberUser.Id,
                    Succeeded = succeeded,
                };
                _producer.Publish(update);
            }
        }
    }
}
