using eShop.TelegramFramework;
using eShop.TelegramFramework.Builders;
using Telegram.Bot;
using eShop.TelegramFramework.UI;
using eShop.Telegram.Models;
using Telegram.Bot.Types;
using eShop.Messaging.Contracts.Distribution.ShopSettings;

namespace eShop.Telegram.TelegramFramework.Views
{
    public class ShopSettingsShopsView : ITelegramView
    {
        private readonly long _chatId;
        private readonly int _messageId;
        private readonly IEnumerable<Shop> _shops;
        private readonly int _page;

        public ShopSettingsShopsView(long chatId, int messageId, IEnumerable<Shop> shops, int page = 0)
        {
            _chatId = chatId;
            _messageId = messageId;
            _shops = shops;
            _page = page;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var text = "Тут ви можете обрати цікаві вам магазини";

            var elements = new List<IInlineKeyboardElement>();
            foreach (var shop in _shops)
            {
                var name = shop.Name;
                var element = new InlineKeyboardToggle($"- {name}", $"+ {name}", shop.IsEnabled, TelegramAction.SetShopSettingsShopState, shop.Id.ToString());
                elements.Add(element);
            }

            var grid = new InlineKeyboardGrid(elements);
            var page = new InlineKeyboardPage(grid, TelegramAction.ShopSettingsShops)
            {
                Index = _page,
                Navigation = new InlineKeyboardNavigation(new InlineKeyboardAction("Назад", TelegramAction.ShopSettings)),
            };
            var replyMarkup = markupBuilder.Build(page);
            await botClient.EditMessageTextAsync(new ChatId(_chatId), _messageId, text, replyMarkup: replyMarkup);
        }
    }
}
