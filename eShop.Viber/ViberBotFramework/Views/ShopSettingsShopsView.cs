using eShop.Bots.Common;
using eShop.Messaging.Contracts.Distribution.ShopSettings;
using eShop.Viber.Models;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views
{
    public class ShopSettingsShopsView : IViberView
    {
        private readonly string _receiverId;
        private readonly IEnumerable<Shop> _shops;

        public ShopSettingsShopsView(string receiverId, IEnumerable<Shop> shops)
        {
            _receiverId = receiverId;
            _shops = shops;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var text = "Тут ви можете обрати цікаві вам магазини";

            var buttons = new List<Button>();

            foreach (var shop in _shops)
            {
                var shopEnabled = shop.IsEnabled;
                var button = new Button
                {
                    Rows = 1,
                    Text = shopEnabled ? $"+ {shop.Name}" : $"- {shop.Name}",
                    ActionBody = botContextConverter.Serialize(ViberAction.SetShopSettingsShopState, shop.Id.ToString(), (!shopEnabled).ToString()),
                };

                buttons.Add(button);
            }

            buttons.Add(new Button
            {
                Rows = 1,
                Text = "Назад",
                ActionBody = botContextConverter.Serialize(ViberAction.ShopSettings),
            });

            var keyboard = new Keyboard
            {
                Buttons = buttons,
            };

            var message = new Message
            {
                Type = MessageType.Text,
                Receiver = _receiverId,
                Text = text,
                Keyboard = keyboard,
            };

            return message;
        }
    }
}
