using eShop.Bots.Common;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views.Registration
{
    public class PhonNumberConfirmedView : IViberView
    {
        public Message Build(IBotContextConverter botContextConverter)
        {
            var replyText = "Дякуємо! Ваш номер телефону успішно підтверждено.\nПоверніться на сторінку реєстрації для продовження роботи із сайтом.";

            var message = new Message
            {
                Type = MessageType.Text,
                Text = replyText,
            };

            return message;
        }
    }
}
