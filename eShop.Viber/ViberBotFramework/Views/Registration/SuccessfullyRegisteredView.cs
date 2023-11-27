using eShop.Bots.Common;
using eShop.Viber.Models;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views.Registration
{
    public class SuccessfullyRegisteredView : IViberView
    {
        private readonly string _receiverId;
        private readonly string _providerEmail;

        public SuccessfullyRegisteredView(string receiverId, string providerEmail)
        {
            _receiverId = receiverId;
            _providerEmail = providerEmail;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var replyText = $"{_providerEmail} встановлений як Ваш постачальник анонсів.";
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
