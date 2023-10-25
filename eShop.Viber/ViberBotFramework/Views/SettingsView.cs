using eShop.Bots.Common;
using eShop.Viber.Models;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views
{
    public class SettingsView : IViberView
    {
        private string _externalId;
        private bool _isEnabled;

        public SettingsView(string externalId, bool isEnabled)
        {
            _externalId = externalId;
            _isEnabled = isEnabled;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var replyText = _isEnabled ? "Надсилання анонсів увімкнено" : "Надсилання анонсів ввимкнено";
            var keyboard = new Keyboard
            {
                Buttons = new[]
                {
                    new Button
                    {
                        Rows = 1,
                        Text = _isEnabled ? "Ввимкнути анонси" : "Увікмнути анонси",
                        ActionBody = botContextConverter.Serialize(_isEnabled ? ViberContext.SettingsDisable : ViberContext.SettingsEnable),
                    },
                    new Button
                    {
                        Rows = 1,
                        Text = "Моя комісія",
                        ActionBody = botContextConverter.Serialize(ViberContext.ComissionSettings),
                    },
                    new Button
                    {
                        Rows = 1,
                        Text = "Мої валюти",
                        ActionBody = botContextConverter.Serialize(ViberContext.CurrencySettings),
                    },
                    new Button
                    {
                        Rows = 1,
                        Text = "Мої магазини",
                        ActionBody = botContextConverter.Serialize(ViberContext.ShopSettings),
                    },
                },
            };

            var message = new Message
            {
                Type = MessageType.Text,
                Receiver = _externalId,
                Text = replyText,
                Keyboard = keyboard,
            };

            return message;
        }
    }
}
