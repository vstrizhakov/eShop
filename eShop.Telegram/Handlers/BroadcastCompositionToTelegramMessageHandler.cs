using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Telegram.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShop.Telegram.Handlers
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
            var telegramChat = await _telegramChatRepository.GetTelegramChatByIdAsync(message.TargetId);
            if (telegramChat != null)
            {
                var messageToSend = message.Message;
                var succeeded = true;
                try
                {
                    var media = new InputMediaPhoto(new InputFileUrl(messageToSend.Image))
                    {
                        Caption = messageToSend.Caption,
                        ParseMode = global::Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                    };
                    await _botClient.SendMediaGroupAsync(new ChatId(telegramChat.ExternalId), new List<IAlbumInputMedia>() { media });
                }
                catch
                {
                    // TODO: Handle 'bot was kicked from chat'
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
                // TODO: Handle absent chats
            }
        }
    }
}
