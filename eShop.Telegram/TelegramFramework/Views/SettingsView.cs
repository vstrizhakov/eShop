using eShop.Messaging.Models.Distribution;
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
        private readonly DistributionSettings _distributionSettings;

        public SettingsView(long chatId, int messageId, DistributionSettings distributionSettings)
        {
            _chatId = chatId;
            _messageId = messageId;
            _distributionSettings = distributionSettings;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var text = "Налаштування";

            var showSales = _distributionSettings.ShowSales;
            var elements = new IInlineKeyboardElement[]
            {
                new InlineKeyboardToggle("Відображати знижки", "Не відображти знижки", showSales, TelegramAction.SetShowSales),
                new InlineKeyboardAction("Моя комісія", TelegramAction.ComissionSettings),
                new InlineKeyboardAction("Мої валюти", TelegramAction.CurrencySettings),
                new InlineKeyboardAction("Мої магазини", TelegramAction.ShopSettings),
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
