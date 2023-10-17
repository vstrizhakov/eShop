using eShop.Telegram.Models;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Builders;
using eShop.TelegramFramework.UI;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShop.Telegram.TelegramFramework.Views
{
    public class WelcomeView : ITelegramView
    {
        private readonly long _chatId;

        public WelcomeView(long chatId)
        {
            _chatId = chatId;
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
            await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
        }
    }
}
