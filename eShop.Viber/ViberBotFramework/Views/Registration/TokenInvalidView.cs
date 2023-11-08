﻿using eShop.Bots.Common;
using eShop.ViberBot;
using eShop.ViberBot.Framework;

namespace eShop.Viber.ViberBotFramework.Views.Registration
{
    public class TokenInvalidView : IViberView
    {
        private readonly string _receiverId;

        public TokenInvalidView(string receiverId)
        {
            _receiverId = receiverId;
        }

        public Message Build(IBotContextConverter botContextConverter)
        {
            var replyText = "Під час підтвердження номеру телефону сталася помилка.\nБудь ласка, оновіть сторінку реєстрації та спробуйте ще раз.";

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
