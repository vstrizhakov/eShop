using eShop.Bots.Common;
using eShop.Viber.Models;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views
{
    public class ComissionSettingsView : IViberView
    {
        private readonly decimal _amount;
        private readonly string _receiverId;

        public ComissionSettingsView(string receiverId, decimal amount)
        {
            _receiverId = receiverId;
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
                        ActionBody = botContextConverter.Serialize(ViberAction.SetComissionAmount),
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
                Text = text,
                Receiver = _receiverId,
                Keyboard = keyboard,
            };

            return message;
        }
    }
}
