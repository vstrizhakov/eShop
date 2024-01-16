using eShopping.Bots.Common;
using eShopping.Messaging.Contracts;
using eShopping.ViberBot;
using eShopping.Viber.Models;
using eShopping.Viber.Repositories;
using MassTransit;

namespace eShopping.Viber.Consumers
{
    public class BroadcastCompositionToViberMessageHandler : IConsumer<BroadcastCompositionToViberMessage>
    {
        private readonly IViberBotClient _botClient;
        private readonly IViberUserRepository _viberUserRepository;
        private readonly IBotContextConverter _botContextConverter;

        public BroadcastCompositionToViberMessageHandler(
            IViberBotClient botClient,
            IViberUserRepository viberUserRepository,
            IBotContextConverter botContextConverter)
        {
            _botClient = botClient;
            _viberUserRepository = viberUserRepository;
            _botContextConverter = botContextConverter;
        }

        public async Task Consume(ConsumeContext<BroadcastCompositionToViberMessage> context)
        {
            var command = context.Message;
            var viberUser = await _viberUserRepository.GetViberUserByIdAsync(command.TargetId);
            if (viberUser != null)
            {
                var messageToSend = command.Message;
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
                                ActionBody = _botContextConverter.Serialize(ViberAction.Settings),
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
                    DistributionId = command.DistributionId,
                    AnnouncerId = command.AnnouncerId,
                    DistributionItemId = command.DistributionItemId,
                    Succeeded = succeeded,
                };
                await context.Publish(update);
            }
            else
            {
                // TODO: handle viber user is absent
            }
        }
    }
}
