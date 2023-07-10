using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Telegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using eShop.Telegram.Repositories;

namespace eShop.Telegram.MessageHandlers
{
    public class TelegramUserCreateAccountResponseMessageHandler : IMessageHandler<TelegramUserCreateAccountResponseMessage>
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ITelegramUserRepository _repository;
        private readonly IBotContextConverter _botContextConverter;

        public TelegramUserCreateAccountResponseMessageHandler(
            ITelegramBotClient botClient,
            ITelegramUserRepository repository,
            IBotContextConverter botContextConverter)
        {
            _botClient = botClient;
            _repository = repository;
            _botContextConverter = botContextConverter;
        }

        public async Task HandleMessageAsync(TelegramUserCreateAccountResponseMessage message)
        {
            var telegramUser = await _repository.GetTelegramUserByIdAsync(message.TelegramUserId);
            if (telegramUser != null)
            {
                await _repository.UpdateAccountIdAsync(telegramUser, message.AccountId);

                var chatId = telegramUser.Chats.Select(e => e.Chat).FirstOrDefault(e => e.Type == ChatType.Private)?.ExternalId;
                if (chatId != null)
                {
                    {
                        var replyMarkup = new ReplyKeyboardRemove();
                        var replyText = $"Вас успішно зареєстровано\n\n{message.ProviderEmail} встановлений як Ваш постачальник анонсів.";

                        await _botClient.SendTextMessageAsync(new ChatId(chatId.Value), replyText, replyMarkup: replyMarkup);
                    }

                    var telegramUserChats = telegramUser.Chats
                        .Where(e => e.Chat.Type == ChatType.Group || e.Chat.Type == ChatType.Channel || e.Chat.Type == ChatType.Supergroup)
                        .Where(e => e.Chat.SupergroupId == null)
                        .Where(e => e.Status == ChatMemberStatus.Creator || e.Status == ChatMemberStatus.Administrator);
                    if (telegramUserChats.Any())
                    {
                        var replyText = "Оберіть групу чи канал, до якої хотіли б налаштувати відправку анонсів:";
                        var replyMarkup = new InlineKeyboardMarkup(telegramUserChats.Select(e =>
                        {
                            var chat = e.Chat;
                            var callbackData = _botContextConverter.Serialize(TelegramContext.Action.SetUpGroup, e.ChatId.ToString());
                            return new List<InlineKeyboardButton>()
                            {
                                InlineKeyboardButton.WithCallbackData(chat.Title!, callbackData),
                            };
                        }));
                        await _botClient.SendTextMessageAsync(new ChatId(chatId.Value), replyText, replyMarkup: replyMarkup);
                    }
                    else
                    {
                        var replyText = "Додайте бота до групи чи каналу, у який хочете налаштувати відправку анонсів, і натисніть кнопку Оновити нижче.";
                        var replyMarkup = new InlineKeyboardMarkup(new List<InlineKeyboardButton>()
                        {
                            new InlineKeyboardButton("Оновити")
                            {
                                CallbackData = _botContextConverter.Serialize(TelegramContext.Action.Refresh),
                            },
                        });
                        await _botClient.SendTextMessageAsync(new ChatId(chatId.Value), replyText, replyMarkup: replyMarkup);
                    }
                }
            }
            else
            {

            }
        }
    }
}
