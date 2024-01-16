using eShopping.Bots.Common;
using eShopping.ViberBot;
using eShopping.ViberBot.Framework;
using eShopping.Viber.Models;

namespace eShopping.Viber.ViberBotFramework.Views
{
    public class SetComissionAmountView : IViberView
    {
        private readonly string _receiverId;
        private readonly decimal _amount;

        public SetComissionAmountView(string receiverId, decimal amount)
        {
            _receiverId = receiverId;
            _amount = amount;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var replyText = $"Ваша поточна комісія: {_amount}%\n\nНадішліть новий розмір комісії.";
            var keyboard = new Keyboard
            {
                Buttons = new[]
                {
                    new Button
                    {
                        Rows = 1,
                        Text = "Назад",
                        ActionBody = botContextConverter.Serialize(ViberAction.ComissionSettings),
                    }
                },
            };

            var message = new Message
            {
                Type = MessageType.Text,
                Receiver = _receiverId,
                Text = replyText,
                Keyboard = keyboard,
            };

            return message;
        }
    }
}
