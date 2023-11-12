using eShop.TelegramFramework.Builders;
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

        public async Task RunAsync(params ITelegramView[] views)
        {
            foreach (var view in views)
            {
                await view.ProcessAsync(_botClient, _markupBuilder);
            }
        }
    }
}
