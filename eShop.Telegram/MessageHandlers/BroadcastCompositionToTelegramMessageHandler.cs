using eShop.Messaging;
using eShop.Messaging.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using eShop.Telegram.Repositories;
using eShop.RabbitMq;
using eShop.Messaging.Extensions;

namespace eShop.Telegram.MessageHandlers
{
    public class BroadcastCompositionToTelegramMessageHandler : IMessageHandler<BroadcastCompositionToTelegramMessage>
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ITelegramChatRepository _telegramChatRepository;
        private readonly IRabbitMqProducer _producer;

        public BroadcastCompositionToTelegramMessageHandler(
            ITelegramBotClient botClient,
            ITelegramChatRepository telegramChatRepository,
            IRabbitMqProducer producer)
        {
            _botClient = botClient;
            _telegramChatRepository = telegramChatRepository;
            _producer = producer;
        }

        public async Task HandleMessageAsync(BroadcastCompositionToTelegramMessage message)
        {
            var telegramChats = await _telegramChatRepository.GetTelegramChatsByIdsAsync(message.TelegramChatIds);
            // TODO: Handle absent chats
            foreach (var telegramChat in telegramChats)
            {
                var succeeded = true;
                try
                {
                    var media = new InputMediaPhoto(new InputFileUrl(message.Image));
                    media.Caption = message.Caption;
                    await _botClient.SendMediaGroupAsync(new ChatId(telegramChat.ExternalId), new List<IAlbumInputMedia>() { media });
                }
                catch
                {
                    succeeded = false;
                }

                var update = new BroadcastCompositionToTelegramUpdateEvent
                {
                    DistributionGroupId = message.DistributionGroupId,
                    TelegramChatId = telegramChat.Id,
                    Succeeded = succeeded,
                };
                _producer.Publish(update);
            }
        }
    }
}
