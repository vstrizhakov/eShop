using eShop.Telegram.Models;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Builders;
using eShop.TelegramFramework.UI;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShop.Telegram.TelegramFramework.Views
{
    public class ComissionSettingsView : ITelegramView
    {
        private readonly long _chatId;
        private readonly decimal _amount;

        public ComissionSettingsView(long chatId, decimal amount)
        {
            _chatId = chatId;
            _amount = amount;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var text = "Налаштування комісій";

            var grid = new InlineKeyboardGrid(new IInlineKeyboardElement[]
            {
                new InlineKeyboardAction($"Розмір комісії ({_amount}%)", TelegramAction.SetComissionAmount),
            });
            var page = new InlineKeyboardPage(grid, TelegramAction.ComissionSettings) 
            {
                Navigation = new InlineKeyboardNavigation(new InlineKeyboardAction("Назад", TelegramAction.Settings)),
            };
            var replyMarkup = markupBuilder.Build(page);

            await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
        }
    }
}
