using eShop.TelegramFramework;
using eShop.TelegramFramework.Builders;
using Telegram.Bot;
using eShop.Messaging.Models.Distribution.ShopSettings;
using eShop.TelegramFramework.UI;
using eShop.Telegram.Models;
using Telegram.Bot.Types;

namespace eShop.Telegram.TelegramFramework.Views
{
    public class ShopSettingsShopsView : ITelegramView
    {
        private readonly long _chatId;
        private readonly IEnumerable<Shop> _shops;

        public ShopSettingsShopsView(long chatId, IEnumerable<Shop> shops)
        {
            _chatId = chatId;
            _shops = shops;
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

            var control = new InlineKeyboardList(elements)
            {
                Navigation = new InlineKeyboardAction("Назад", TelegramAction.ShopSettings),
            };
            var replyMarkup = markupBuilder.Build(control);
            await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
        }
    }
}
