using eShop.TelegramFramework.UI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.TelegramFramework.Builders
{
    internal class InlineKeyboardListBuilder : IInlineKeyboardMarkupBuilder<InlineKeyboardList>
    {
        private readonly IInlineKeyboardButtonBuilder _buttonBuilder;

        public InlineKeyboardListBuilder(IInlineKeyboardButtonBuilder buttonBuilder)
        {
            _buttonBuilder = buttonBuilder;
        }

        public InlineKeyboardMarkup Build(InlineKeyboardList control)
        {
            var buttons = new List<IEnumerable<InlineKeyboardButton>>();
            var maxButtons = 10;
            var maxCurrencies = maxButtons - 1;
            foreach (var item in control.Items)
            {
                var button = _buttonBuilder.Build(item);

                buttons.Add(new[] { button });
            }

            var navigation = control.Navigation;
            if (navigation != null)
            {
                var button = _buttonBuilder.Build(navigation);

                buttons.Add(new[] { button });
            }

            var replyMarkup = new InlineKeyboardMarkup(buttons);
            return replyMarkup;
        }
    }
}
