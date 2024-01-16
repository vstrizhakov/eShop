using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Builders;
using eShopping.TelegramFramework.UI;
using eShopping.Telegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShopping.Telegram.TelegramFramework.Views
{
    public class WelcomeView : ITelegramView
    {
        private readonly long _chatId;
        private readonly int? _messageId;

        public WelcomeView(long chatId, int? messageId)
        {
            _chatId = chatId;
            _messageId = messageId;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var text = "Доброго дня!";

            var elements = new IInlineKeyboardElement[]
            {
                new InlineKeyboardAction("Мої групи", TelegramAction.MyGroups),
                new InlineKeyboardAction("Налаштування", TelegramAction.Settings),
            };
            var control = new InlineKeyboardGrid(elements);

            var replyMarkup = markupBuilder.Build(control);
            if (!_messageId.HasValue)
            {
                await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
            }
            else
            {
                await botClient.EditMessageTextAsync(new ChatId(_chatId), _messageId.Value, text, replyMarkup: replyMarkup);
            }
        }
    }
}
