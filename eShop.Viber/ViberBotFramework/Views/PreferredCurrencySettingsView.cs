using eShop.Bots.Common;
using eShop.Viber.Models;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views
{
    public class PreferredCurrencySettingsView : IViberView
    {
        private readonly string _receiverId;
        private readonly IEnumerable<Messaging.Models.Currency> _currencies;

        public PreferredCurrencySettingsView(string receiverId, IEnumerable<Messaging.Models.Currency> currencies)
        {
            _receiverId = receiverId;
            _currencies = currencies;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var text = "Оберіть одну з наступних валют як основу. Вона буде відображатися в ваших анонсах.";

            var buttons = new List<Button>();

            foreach (var currency in _currencies)
            {
                var button = new Button
                {
                    Rows = 1,
                    Text = currency.Name,
                    ActionBody = botContextConverter.Serialize(ViberContext.SetPreferredCurrency, currency.Id.ToString()),
                };

                buttons.Add(button);
            }

            buttons.Add(new Button
            {
                Rows = 1,
                Text = "Назад",
                ActionBody = botContextConverter.Serialize(ViberContext.CurrencySettings),
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
