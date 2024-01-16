using eShopping.Bots.Common;
using eShopping.Messaging.Contracts.Distribution;
using eShopping.ViberBot;
using eShopping.ViberBot.Framework;
using eShopping.Viber.Models;

namespace eShopping.Viber.ViberBotFramework.Views
{
    public class CurrencyRateSettingsView : IViberView
    {
        private readonly string _receiverId;
        private readonly Messaging.Contracts.Currency _preferredCurrency;
        private readonly IEnumerable<CurrencyRate> _currencyRates;

        public CurrencyRateSettingsView(string receiverId, Messaging.Contracts.Currency preferredCurrency, IEnumerable<CurrencyRate> currencyRates)
        {
            _receiverId = receiverId;
            _preferredCurrency = preferredCurrency;
            _currencyRates = currencyRates;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var text = $"Поточний курс валют відносно {_preferredCurrency.Name}\n\nНатисніть на валюту зі списку нижче, щоб змінити її курс.";
            var buttons = new List<Button>();

            foreach (var currencyRate in _currencyRates)
            {
                var currency = currencyRate.Currency;
                var button = new Button
                {
                    Rows = 1,
                    Text = $"{currency.Name}: {currencyRate.Rate}",
                    ActionBody = botContextConverter.Serialize(ViberAction.SetCurrencyRate, currency.Id.ToString()),
                };

                buttons.Add(button);
            }

            buttons.Add(new Button
            {
                Rows = 1,
                Text = "Назад",
                ActionBody = botContextConverter.Serialize(ViberAction.CurrencySettings),
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
