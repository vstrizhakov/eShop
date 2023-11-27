using eShop.Bots.Common;
using eShop.Messaging.Contracts.Distribution;
using eShop.Viber.Models;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views
{
    public class SettingsView : IViberView
    {
        private readonly string _chatId;
        private readonly bool _isEnabled;
        private readonly DistributionSettings _distributionSettings;

        public SettingsView(string chatId, bool isEnabled, DistributionSettings distributionSettings)
        {
            _chatId = chatId;
            _isEnabled = isEnabled;
            _distributionSettings = distributionSettings;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var replyText = _isEnabled ? "Надсилання анонсів увімкнено" : "Надсилання анонсів ввимкнено";
            var showSales = _distributionSettings.ShowSales;
            var keyboard = new Keyboard
            {
                Buttons = new[]
                {
                    new Button
                    {
                        Rows = 1,
                        Text = _isEnabled ? "Ввимкнути анонси" : "Увікмнути анонси",
                        ActionBody = botContextConverter.Serialize(ViberAction.SetIsChatEnalbed, (!_isEnabled).ToString()),
                    },
                    new Button
                    {
                        Rows = 1,
                        Text = showSales ? "Не відображати знижки" : "Відображати знижки",
                        ActionBody = botContextConverter.Serialize(ViberAction.SetShowSales, (!showSales).ToString()),
                    },
                    new Button
                    {
                        Rows = 1,
                        Text = "Моя комісія",
                        ActionBody = botContextConverter.Serialize(ViberAction.ComissionSettings),
                    },
                    new Button
                    {
                        Rows = 1,
                        Text = "Мої валюти",
                        ActionBody = botContextConverter.Serialize(ViberAction.CurrencySettings),
                    },
                    new Button
                    {
                        Rows = 1,
                        Text = "Мої магазини",
                        ActionBody = botContextConverter.Serialize(ViberAction.ShopSettings),
                    },
                },
            };

            var message = new Message
            {
                Type = MessageType.Text,
                Receiver = _chatId,
                Text = replyText,
                Keyboard = keyboard,
            };

            return message;
        }
    }
}
