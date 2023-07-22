using eShop.Messaging;
using eShop.Messaging.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using eShop.Telegram.Repositories;
using eShop.Messaging.Extensions;

namespace eShop.Telegram.MessageHandlers
{
    public class BroadcastCompositionToTelegramMessageHandler : IMessageHandler<BroadcastCompositionToTelegramMessage>
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ITelegramChatRepository _telegramChatRepository;
        private readonly IProducer _producer;

        public BroadcastCompositionToTelegramMessageHandler(
            ITelegramBotClient botClient,
            ITelegramChatRepository telegramChatRepository,
            IProducer producer)
        {
            _botClient = botClient;
            _telegramChatRepository = telegramChatRepository;
            _producer = producer;
        }

        public async Task HandleMessageAsync(BroadcastCompositionToTelegramMessage message)
        {
            var requests = message.Requests;
            var telegramChats = await _telegramChatRepository.GetTelegramChatsByIdsAsync(requests.Select(e => e.TargetId));
            // TODO: Handle absent chats

            var messageToSend = message.Message;
            foreach (var telegramChat in telegramChats)
            {
                var request = requests.FirstOrDefault(e => e.TargetId == telegramChat.Id);
                var succeeded = true;
                try
                {
                    var media = new InputMediaPhoto(new InputFileUrl(messageToSend.Image));
                    media.Caption = messageToSend.Caption;
                    await _botClient.SendMediaGroupAsync(new ChatId(telegramChat.ExternalId), new List<IAlbumInputMedia>() { media });
                }
                catch
                {
                    succeeded = false;
                }

                var update = new BroadcastMessageUpdateEvent
                {
                    RequestId = request.RequestId,
                    Succeeded = succeeded,
                };
                _producer.Publish(update);
            }
        }
    }
}
