using eShop.Bots.Common;
using eShop.Viber.Models;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views.Registration
{
    public class PhoneNumberInvalidView : IViberView
    {
        private readonly string _receiverId;

        public PhoneNumberInvalidView(string receiverId)
        {
            _receiverId = receiverId;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var replyText = "Ваш номер телефону відрізняється від того, який Ви вказали під час реєстрації.\nБудь ласка, зареєструйтеся під поточним номером, або підтвердьте номер телефону з відповідного Viber-акаунту.";

            var message = new Message
            {
                Type = MessageType.Text,
                Receiver = _receiverId,
                Text = replyText,
            };

            return message;
        }
    }
}
