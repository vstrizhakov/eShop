using eShop.Bots.Common;
using eShop.Telegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using eShop.Telegram.Entities;

namespace eShop.Telegram.Inner.Views
{
    public class RefreshView : ITelegramView
    {
        private readonly long _chatId;
        private readonly IEnumerable<TelegramChatMember> _telegramUserChats;

        public RefreshView(long chatId, IEnumerable<TelegramChatMember> telegramUserChats)
        {
            _chatId = chatId;
            _telegramUserChats = telegramUserChats;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            if (_telegramUserChats.Any())
            {
                var replyText = "Оберіть групу чи канал, до якої хотіли б налаштувати відправку анонсів:";
                var replyMarkup = new InlineKeyboardMarkup(_telegramUserChats.Select(e =>
                {
                    var chat = e.Chat;
                    return new List<InlineKeyboardButton>
                    {
                        new InlineKeyboardButton(chat.Title!)
                        {
                            CallbackData = botContextConverter.Serialize(TelegramAction.SetUpGroup, e.ChatId.ToString()),
                        }
                    };
                }));
                await botClient.SendTextMessageAsync(new ChatId(_chatId), replyText, replyMarkup: replyMarkup);
            }
            else
            {
                var replyText = "Додайте бота до групи чи каналу, у який хочете налаштувати відправку анонсів, і натисніть кнопку Оновити нижче.";
                var replyMarkup = new InlineKeyboardMarkup(new List<InlineKeyboardButton>()
                {
                    new InlineKeyboardButton("Оновити")
                    {
                        CallbackData = botContextConverter.Serialize(TelegramAction.Refresh),
                    },
                });
                await botClient.SendTextMessageAsync(new ChatId(_chatId), replyText, replyMarkup: replyMarkup);
            }
        }
    }
}
