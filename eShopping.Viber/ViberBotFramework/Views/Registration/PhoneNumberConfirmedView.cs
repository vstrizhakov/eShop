using eShopping.Bots.Common;
using eShopping.ViberBot;
using eShopping.ViberBot.Framework;

namespace eShopping.Viber.ViberBotFramework.Views.Registration
{
    public class PhoneNumberConfirmedView : IViberView
    {
        private string _receiverId;

        public PhoneNumberConfirmedView(string receiverId)
        {
            _receiverId = receiverId;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var replyText = "Дякуємо! Ваш номер телефону успішно підтверждено.\nПоверніться на сторінку реєстрації для продовження роботи із сайтом.";

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
