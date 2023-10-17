using eShop.Telegram.Models;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Builders;
using eShop.TelegramFramework.UI;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShop.Telegram.TelegramFramework.Views
{
    public class SettingsView : ITelegramView
    {
        private readonly long _chatId;
        private readonly int _messageId;

        public SettingsView(long chatId, int messageId)
        {
            _chatId = chatId;
            _messageId = messageId;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var text = "Налаштування";

            var elements = new IInlineKeyboardElement[]
            {
                new InlineKeyboardAction("Мої магазини", TelegramAction.ShopSettings),
                new InlineKeyboardAction("Мої валюти", TelegramAction.CurrencySettings),
                new InlineKeyboardAction("Моя комісія", TelegramAction.ComissionSettings),
            };
            var grid = new InlineKeyboardGrid(elements);
            var page = new InlineKeyboardPage(grid, TelegramAction.Settings)
            {
                Navigation = new InlineKeyboardNavigation(new InlineKeyboardAction("Назад", TelegramAction.Home)),
            };

            var replyMarkup = markupBuilder.Build(page);
            await botClient.EditMessageTextAsync(new ChatId(_chatId), _messageId, text, replyMarkup: replyMarkup);
        }
    }
}
