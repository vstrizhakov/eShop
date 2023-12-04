using eShop.Bots.Common;
using eShop.Messaging.Contracts.Distribution;
using eShop.Viber.Models;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views.Registration
{
    public class SuccessfullyRegisteredView : IViberView
    {
        private readonly string _receiverId;
        private readonly Announcer? _announcer;

        public SuccessfullyRegisteredView(string receiverId, Announcer announcer)
        {
            _receiverId = receiverId;
            _announcer = announcer;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var replyText = $"Вас успішно зареєстровано!";
            if (_announcer != null)
            {
                replyText += $"\n\n{_announcer.FirstName}";
                var lastName = _announcer.LastName;
                if (lastName != null)
                {
                    replyText += $" {lastName}";
                }
                replyText += " встановлений як Ваш постачальник анонсів.";
            }

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
                MinApiVersion = 7,
                Text = replyText,
                Keyboard = keyboard,
            };

            return message;
        }
    }
}
