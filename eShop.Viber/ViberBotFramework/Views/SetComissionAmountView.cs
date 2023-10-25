using eShop.Bots.Common;
using eShop.Viber.Models;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views
{
    public class SetComissionAmountView : IViberView
    {
        private string _receiverId;
        private decimal _amount;

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
                        ActionBody = botContextConverter.Serialize(ViberContext.ComissionSettings),
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
