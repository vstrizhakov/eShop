using eShop.Bots.Common;
using eShop.Telegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using eShop.Telegram.Entities;

namespace eShop.Telegram.Inner.Views
{
    public class MyGroupsView : ITelegramView
    {
        private readonly long _chatId;
        private readonly IEnumerable<TelegramChatMember> _telegramUserChats;

        public MyGroupsView(long chatId, IEnumerable<TelegramChatMember> telegramUserChats)
        {
            _chatId = chatId;
            _telegramUserChats = telegramUserChats;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            if (_telegramUserChats.Any())
            {
                var replyText = "Оберіть групу чи канал, до якої хотіли б налаштувати відправку анонсів:";

                var buttons = new List<IEnumerable<InlineKeyboardButton>>();
                foreach (var chatMember in _telegramUserChats)
                {
                    var chat = chatMember.Chat;
                    var button = new InlineKeyboardButton(chat.Title!)
                    {
                        CallbackData = botContextConverter.Serialize(TelegramAction.SetUpGroup, chat.Id.ToString()),
                    };
                    buttons.Add(new[] { button });
                }

                var backButton = new InlineKeyboardButton("Назад")
                {
                    CallbackData = botContextConverter.Serialize(TelegramAction.Home),
                };
                buttons.Add(new[] { backButton });

                var replyMarkup = new InlineKeyboardMarkup(buttons);

                await botClient.SendTextMessageAsync(new ChatId(_chatId), replyText, replyMarkup: replyMarkup);
            }
            else
            {
                var replyText = "Додайте бота до групи чи каналу, у який хочете налаштувати відправку анонсів, і натисніть кнопку Оновити нижче.";
                var replyMarkup = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        new InlineKeyboardButton("Оновити")
                        {
                            CallbackData = botContextConverter.Serialize(TelegramAction.Refresh),
                        },
                    },
                    new[]
                    {
                        new InlineKeyboardButton("Назад")
                        {
                            CallbackData = botContextConverter.Serialize(TelegramAction.Home),
                        },
                    },
                });

                await botClient.SendTextMessageAsync(new ChatId(_chatId), replyText, replyMarkup: replyMarkup);
            }
        }
    }
}
