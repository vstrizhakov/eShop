using eShop.Bots.Common;
using eShop.Viber.Models;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views
{
    public class ComissionSettingsView : IViberView
    {
        private readonly double _amount;
        private readonly string _receiverId;

        public ComissionSettingsView(string externalId, double amount)
        {
            _receiverId = externalId;
            _amount = amount;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var text = "Налаштування комісій";
            var keyboard = new Keyboard
            {
                Buttons = new[]
                {
                    new Button
                    {
                        Rows = 1,
                        Text = $"Розмір комісії ({_amount}%)",
                        ActionBody = botContextConverter.Serialize(ViberContext.SetComissionAmount),
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
                Text = text,
                Receiver = _receiverId,
                Keyboard = keyboard,
            };

            return message;
        }
    }
}
