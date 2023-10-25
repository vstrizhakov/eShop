using eShop.Bots.Common;
using eShop.Viber.Models;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views
{
    public class CurrencySettingsView : IViberView
    {
        private readonly string _receiverId;
        private readonly Messaging.Models.Currency? _preferredCurrency;

        public CurrencySettingsView(string receiverId, Messaging.Models.Currency? preferredCurrency)
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
                        ActionBody = botContextConverter.Serialize(ViberContext.PreferredCurrencySettings),
                    },
                    new Button
                    {
                        Rows = 1,
                        Text = "Курси валют",
                        ActionBody = botContextConverter.Serialize(ViberContext.CurrencyRateSettings),
                    },
                    new Button
                    {
                        Rows = 1,
                        Text = "Назад",
                        ActionBody = botContextConverter.Serialize(ViberContext.Settings),
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
