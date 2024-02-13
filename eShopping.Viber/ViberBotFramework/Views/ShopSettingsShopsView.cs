using eShopping.Bots.Common;
using eShopping.Messaging.Contracts.Distribution.ShopSettings;
using eShopping.ViberBot;
using eShopping.ViberBot.Framework;
using eShopping.Viber.Models;

namespace eShopping.Viber.ViberBotFramework.Views
{
    public class ShopSettingsShopsView : IViberView
    {
        private const int MaxRows = 24;
        private const int MaxColumns = 6;

        private readonly string _receiverId;
        private readonly IEnumerable<Shop> _shops;
        private readonly int _page;

        public ShopSettingsShopsView(string receiverId, IEnumerable<Shop> shops, int page)
        {
            _receiverId = receiverId;
            _shops = shops;
            _page = page;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var text = "Тут ви можете обрати цікаві вам магазини";

            var buttons = new List<Button>();

            var rows = MaxRows;
            rows -= 1; // Back button
            var isPaginationRequired = _shops.Count() > rows;
            if (isPaginationRequired)
            {
                rows -= 1; // Pagination row
            }

            var skip = rows * _page;
            foreach (var shop in _shops.Skip(skip).Take(rows))
            {
                var shopEnabled = shop.IsEnabled;
                var button = new Button
                {
                    Rows = 1,
                    Text = shopEnabled ? $"+ {shop.Name}" : $"- {shop.Name}",
                    ActionBody = botContextConverter.Serialize(ViberAction.SetShopSettingsShopState, shop.Id.ToString(), (!shopEnabled).ToString(), _page.ToString()),
                };

                buttons.Add(button);
            }

            if (isPaginationRequired)
            {
                var pages = (int)Math.Ceiling(_shops.Count() / (double)rows);
                var cols = MaxColumns;
                var isPagePaginationRequired = pages > MaxColumns;

                isPagePaginationRequired = false; // TEMP

                if (isPagePaginationRequired)
                {
                    cols -= 2;
                }

                if (isPagePaginationRequired)
                {
                    var button = new Button
                    {
                        Columns = 1,
                        Text = "<",
                        ActionBody = botContextConverter.Serialize(ViberAction.ShopSettingsShops, "<"),
                    };

                    buttons.Add(button);
                }

                var pagesToDisplay = Math.Min(pages, cols);
                for (int i = 0; i < pagesToDisplay; i++)
                {
                    var button = new Button
                    {
                        Columns = 1,
                        Text = $"{i + 1}",
                        ActionBody = botContextConverter.Serialize(ViberAction.ShopSettingsShops, i.ToString()),
                    };

                    buttons.Add(button);
                }

                if (isPagePaginationRequired)
                {
                    var button = new Button
                    {
                        Columns = 1,
                        Text = ">",
                        ActionBody = botContextConverter.Serialize(ViberAction.ShopSettingsShops, ">"),
                    };

                    buttons.Add(button);
                }
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
