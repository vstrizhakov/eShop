using eShopping.Bots.Common;
using eShopping.Messaging.Contracts.Distribution;
using eShopping.ViberBot;
using eShopping.ViberBot.Framework;
using eShopping.Viber.Models;

namespace eShopping.Viber.ViberBotFramework.Views.Registration
{
    public class SubscribedToAnnouncerView : IViberView
    {
        private readonly string _receiverId;
        private readonly Announcer _announcer;

        public SubscribedToAnnouncerView(string receiverId, Announcer announcer)
        {
            _receiverId = receiverId;
            _announcer = announcer;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var announcerName = _announcer.FirstName;
            var announcerLastName = _announcer.LastName;
            if (announcerLastName != null)
            {
                announcerName += $" {announcerLastName}";
            }

            var replyText = $"{announcerName} успішно встановлений як ваш постачальник анонсів.";
            var keyboard = new Keyboard
            {
                Buttons = new[]
                {
                    new Button
                    {
                        Rows = 1,
                        Text = "Налаштування анонсів",
                        ActionBody = botContextConverter.Serialize(ViberAction.Settings),
                    },
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
