using eShop.Bots.Common;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views.Registration
{
    public class FinishRegistrationView : IViberView
    {
        private readonly string _receiverId;

        public FinishRegistrationView(string receiverId)
        {
            _receiverId = receiverId;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var message = new Message
            {
                Type = MessageType.Text,
                Receiver = _receiverId,
                Text = "Для завершення реєстрації надішліть свій номер телефону, будь ласка",
                MinApiVersion = 7,
                Keyboard = new Keyboard
                {
                    Buttons = new[]
                    {
                        new Button
                        {
                            Text = "Надіслати номер телефону",
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
