using eShop.Bots.Common;
using eShop.Telegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using eShop.Telegram.Entities;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Builders;
using eShop.TelegramFramework.UI;

namespace eShop.Telegram.TelegramFramework.Views
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

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            if (_telegramUserChats.Any())
            {
                var text = "Оберіть групу чи канал, до якої хотіли б налаштувати відправку анонсів:";

                var elements = new List<IInlineKeyboardElement>();
                foreach (var chatMember in _telegramUserChats)
                {
                    var chat = chatMember.Chat;
                    var element = new InlineKeyboardAction(chat.Title!, TelegramAction.SetUpGroup, chat.Id.ToString());
                    elements.Add(element);
                }

                var control = new InlineKeyboardList(elements)
                {
                    Navigation = new InlineKeyboardAction("Назад", TelegramAction.Home),
                };

                var replyMarkup = markupBuilder.Build(control);
                await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
            }
            else
            {
                var text = "Додайте бота до групи чи каналу, у який хочете налаштувати відправку анонсів, і натисніть кнопку Оновити нижче.";

                var elements = new IInlineKeyboardElement[]
                {
                    new InlineKeyboardAction("Оновити", TelegramAction.Refresh)
                };
                var control = new InlineKeyboardList(elements)
                {
                    Navigation = new InlineKeyboardAction("Назад", TelegramAction.Home),
                };

                var replyMarkup = markupBuilder.Build(control);
                await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
            }
        }
    }
}
