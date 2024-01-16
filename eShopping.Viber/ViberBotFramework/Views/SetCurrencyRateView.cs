using eShopping.Bots.Common;
using eShopping.Messaging.Contracts.Distribution;
using eShopping.ViberBot;
using eShopping.ViberBot.Framework;
using eShopping.Viber.Models;

namespace eShopping.Viber.ViberBotFramework.Views
{
    public class SetCurrencyRateView : IViberView
    {
        private string _receiverId;
        private Messaging.Contracts.Currency _preferredCurrency;
        private CurrencyRate _currencyRate;

        public SetCurrencyRateView(string receiverId, Messaging.Contracts.Currency preferredCurrency, CurrencyRate currencyRate)
        {
            _receiverId = receiverId;
            _preferredCurrency = preferredCurrency;
            _currencyRate = currencyRate;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var sourceCurrency = _currencyRate.Currency;
            var text = $"{sourceCurrency.Name} -> {_preferredCurrency.Name}\n\nПоточний курс: {_currencyRate.Rate}\n\nБудь ласка, надішліть новий курс для цієї пари.";

            var keyboard = new Keyboard
            {
                Buttons = new[]
                {
                    new Button
                    {
                        Rows = 1,
                        Text = "Назад",
                        ActionBody = botContextConverter.Serialize(ViberAction.CurrencyRateSettings),
                    },
                },
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
