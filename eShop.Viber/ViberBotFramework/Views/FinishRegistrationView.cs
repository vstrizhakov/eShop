using eShop.Bots.Common;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views
{
    public class FinishRegistrationView : IViberView
    {
        public Message Build(IBotContextConverter botContextConverter)
        {
            var message = new Message
            {
                Type = MessageType.Text,
                Text = "Для завершення реєстрації надішліть свій номер телефону, будь ласка",
                MinApiVersion = 7,
                Keyboard = new Keyboard
                {
                    Buttons = new[]
                    {
                        new Button
                        {
                            Text = "Відправити номер телефону",
                            ActionType = "share-phone",
                            ActionBody = string.Empty,
                        },
                    },
                },
            };

            return message;
        }
    }
}
