using eShop.Bots.Common;
using eShop.Viber.Models;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views.Registration
{
    public class AlreadyRegisteredView : IViberView
    {
        public Message Build(IBotContextConverter botContextConverter)
        {
            var replyText = "Ви вже зареєстровані та маєтє встановленного постачальника анонсів";
            var keyboard = new Keyboard
            {
                Buttons = new[]
                {
                    new Button
                    {
                        Rows = 1,
                        Text = "Налаштування анонсів",
                        ActionBody = botContextConverter.Serialize(ViberContext.Settings),
                    },
                },
            };

            var message = new Message
            {
                Type = MessageType.Text,
                Text = replyText,
                Keyboard = keyboard,
            };

            return message;
        }
    }
}
