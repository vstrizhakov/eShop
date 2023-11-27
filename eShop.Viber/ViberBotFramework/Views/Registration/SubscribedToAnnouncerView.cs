using eShop.Bots.Common;
using eShop.Messaging.Contracts.Distribution;
using eShop.Viber.Models;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views.Registration
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
