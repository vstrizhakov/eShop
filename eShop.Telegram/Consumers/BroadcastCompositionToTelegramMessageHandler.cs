using eShop.Messaging.Contracts;
using eShop.Messaging.Contracts;
using eShop.Telegram.Repositories;
using eShop.Telegram.Services;
using MassTransit;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eShop.Telegram.Consumers
{
    public class BroadcastCompositionToTelegramMessageHandler : IConsumer<BroadcastCompositionToTelegramMessage>
    {
        private readonly IRateLimitedTelegramBotClient _botClient;
        private readonly ITelegramChatRepository _telegramChatRepository;

        public BroadcastCompositionToTelegramMessageHandler(
            IRateLimitedTelegramBotClient botClient,
            ITelegramChatRepository telegramChatRepository)
        {
            _botClient = botClient;
            _telegramChatRepository = telegramChatRepository;
        }

        public async Task Consume(ConsumeContext<BroadcastCompositionToTelegramMessage> context)
        {
            var command = context.Message;
            var telegramChat = await _telegramChatRepository.GetTelegramChatByIdAsync(command.TargetId);
            if (telegramChat != null)
            {
                var messageToSend = command.Message;
                var succeeded = true;
                try
                {
                    var media = new InputMediaPhoto(new InputFileUrl(messageToSend.Image))
                    {
                        Caption = messageToSend.Caption,
                        ParseMode = ParseMode.MarkdownV2,
                    };

                    var chatId = new ChatId(telegramChat.ExternalId);
                    var isGroup = telegramChat.Type == ChatType.Group;

                    await _botClient.SendRequestAsync(chatId, isGroup, botClient =>
                        botClient.SendMediaGroupAsync(chatId, new List<IAlbumInputMedia>() { media }));
                }
                catch
                {
                    // TODO: Handle 'bot was kicked from chat'
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
                // TODO: Handle absent chats
            }
        }
    }
}
