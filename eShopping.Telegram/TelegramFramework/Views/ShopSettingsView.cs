﻿using eShopping.Messaging.Contracts.Distribution.ShopSettings;
using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Builders;
using eShopping.TelegramFramework.UI;
using eShopping.Telegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShopping.Telegram.TelegramFramework.Views
{
    public class ShopSettingsView : ITelegramView
    {
        private readonly long _chatId;
        private readonly int _messageId;
        private readonly ShopSettings _shopSettings;

        public ShopSettingsView(long chatId, int messageId, ShopSettings shopSettings)
        {
            _chatId = chatId;
            _messageId = messageId;
            _shopSettings = shopSettings;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var text = "Налаштування магазинів\n\nУвімкніть фільтрування магазинів, а потім в меню \"Магазини\" оберіть ті, що вам потрібні.";

            var elements = new List<IInlineKeyboardElement>();
            var filter = _shopSettings.Filter;

            elements.Add(new InlineKeyboardToggle("Увімкнути фільтрування", "Вимкнути фільтрування", filter, TelegramAction.SetShopSettingsFilter));

            if (filter)
            {
                elements.Add(new InlineKeyboardAction("Магазини", TelegramAction.ShopSettingsShops));
            }

            var grid = new InlineKeyboardGrid(elements);
            var page = new InlineKeyboardPage(grid, TelegramAction.ShopSettings)
            {
                Navigation = new InlineKeyboardNavigation(new InlineKeyboardAction("Назад", TelegramAction.Settings)),
            };
            var replyMarkup = markupBuilder.Build(page);

            await botClient.EditMessageTextAsync(new ChatId(_chatId), _messageId, text, replyMarkup: replyMarkup);
        }
    }
}
