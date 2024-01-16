using eShopping.Bots.Common;
using eShopping.ViberBot;
using eShopping.ViberBot.Framework;
using eShopping.Viber.Models;

namespace eShopping.Viber.ViberBotFramework.Views
{
    public class CurrencySettingsView : IViberView
    {
        private readonly string _receiverId;
        private readonly Messaging.Contracts.Currency? _preferredCurrency;

        public CurrencySettingsView(string receiverId, Messaging.Contracts.Currency? preferredCurrency)
        {
            _receiverId = receiverId;
            _preferredCurrency = preferredCurrency;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var text = "Мої валюти";
            var keyboard = new Keyboard
            {
                Buttons = new[]
                {
                    new Button
                    {
                        Rows = 1,
                        Text = _preferredCurrency != null ? $"Основна валюта ({_preferredCurrency.Name})" : "Основна валюта",
                        ActionBody = botContextConverter.Serialize(ViberAction.PreferredCurrencySettings),
                    },
                    new Button
                    {
                        Rows = 1,
                        Text = "Курси валют",
                        ActionBody = botContextConverter.Serialize(ViberAction.CurrencyRateSettings),
                    },
                    new Button
                    {
                        Rows = 1,
                        Text = "Назад",
                        ActionBody = botContextConverter.Serialize(ViberAction.Settings),
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
