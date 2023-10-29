using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Viber.Models;
using eShop.Viber.Repositories;
using eShop.ViberBot;

namespace eShop.Viber.Handlers
{
    public class BroadcastCompositionToViberMessageHandler : IMessageHandler<BroadcastCompositionToViberMessage>
    {
        private readonly IViberBotClient _botClient;
        private readonly IViberUserRepository _viberUserRepository;
        private readonly IBotContextConverter _botContextConverter;
        private readonly IProducer _producer;

        public BroadcastCompositionToViberMessageHandler(
            IViberBotClient botClient,
            IViberUserRepository viberUserRepository,
            IBotContextConverter botContextConverter,
            IProducer producer)
        {
            _botClient = botClient;
            _viberUserRepository = viberUserRepository;
            _botContextConverter = botContextConverter;
            _producer = producer;
        }

        public async Task HandleMessageAsync(BroadcastCompositionToViberMessage message)
        {
            var viberUser = await _viberUserRepository.GetViberUserByIdAsync(message.TargetId);
            if (viberUser != null)
            {
                var messageToSend = message.Message;
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
                    await _botClient.SendPictureMessageAsync(viberUser.ExternalId, sender, messageToSend.Image.ToString(), messageToSend.Caption, keyboard: keyboard);
                }
                catch
                {
                    succeeded = false;
                }

                var update = new BroadcastMessageUpdateEvent
                {
                    RequestId = message.RequestId,
                    Succeeded = succeeded,
                };
                _producer.Publish(update);
            }
            else
            {
                // TODO: handle viber user is absent
            }
        }
    }
}
