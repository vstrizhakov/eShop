using eShop.Bots.Common;
using eShop.TelegramFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.TelegramFramework.Builders
{
    internal class InlineKeyboardActionBuilder : IInlineKeyboardButtonBuilder<InlineKeyboardAction>
    {
        private readonly IBotContextConverter _botContextConverter;

        public InlineKeyboardActionBuilder(IBotContextConverter botContextConverter)
        {
            _botContextConverter = botContextConverter;
        }

        public InlineKeyboardButton Build(InlineKeyboardAction element)
        {
            var args = new List<string>();
            args.Add(element.Action);
            args.AddRange(element.Arguments);
            var button = new InlineKeyboardButton(element.Caption)
            {
                CallbackData = _botContextConverter.Serialize(args.ToArray()),
            };

            return button;
        }
    }
}
