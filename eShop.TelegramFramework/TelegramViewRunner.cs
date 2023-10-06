using eShop.Bots.Common;
using eShop.TelegramFramework.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace eShop.TelegramFramework
{
    internal class TelegramViewRunner : ITelegramViewRunner
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IInlineKeyboardMarkupBuilder _markupBuilder;

        public TelegramViewRunner(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            _botClient = botClient;
            _markupBuilder = markupBuilder;
        }

        public async Task RunAsync(ITelegramView view)
        {
            await view.ProcessAsync(_botClient, _markupBuilder);
        }
    }
}
