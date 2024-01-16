using eShopping.Bots.Common;
using eShopping.Messaging.Contracts.Distribution.ShopSettings;
using eShopping.ViberBot;
using eShopping.ViberBot.Framework;
using eShopping.Viber.Models;

namespace eShopping.Viber.ViberBotFramework.Views
{
    public class ShopSettingsView : IViberView
    {
        private readonly string _receiverId;
        private readonly ShopSettings _shopSettings;

        public ShopSettingsView(string receiverId, ShopSettings shopSettings)
        {
            _receiverId = receiverId;
            _shopSettings = shopSettings;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var text = "Налаштування магазинів\n\nУвімкніть фільтрування магазинів, а потім в меню \"Магазини\" оберіть ті, що вам потрібні.";

            var filter = _shopSettings.Filter;

            var buttons = new List<Button>
            {
                new Button
                {
                    Rows = 1,
                    Text = filter ? "Ввимкнути фільтрування" : "Увімкнути фільтрування",
                    ActionBody = botContextConverter.Serialize(ViberAction.SetShopSettingsFilter, (!filter).ToString()),
                }
            };

            if (filter)
            {
                buttons.Add(new Button
                {
                    Rows = 1,
                    Text = "Магазини",
                    ActionBody = botContextConverter.Serialize(ViberAction.ShopSettingsShops),
                });
            }

            buttons.Add(new Button
            {
                Rows = 1,
                Text = "Назад",
                ActionBody = botContextConverter.Serialize(ViberAction.Settings),
            });

            var keyboard = new Keyboard
            {
                Buttons = buttons,
            };

            var message = new Message
            {
                Type = MessageType.Text,
                Text = text,
                Receiver = _receiverId,
                Keyboard = keyboard,
            };

            return message;
        }
    }
}
