using eShop.Telegram.Services;
using eShop.TelegramFramework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eShop.Telegram.TelegramFramework
{
    public class TelegramContextStore : IContextStore
    {
        private readonly ITelegramService _telegramService;

        public TelegramContextStore(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        public async Task<string?> GetActiveContextAsync(Update update)
        {
            var activeContext = default(string);
            if (update.Type == UpdateType.Message)
            {
                var from = update.Message!.From;
                if (from != null)
                {
                    var user = await _telegramService.GetUserByExternalIdAsync(from.Id);
                    if (user != null)
                    {
                        activeContext = user.ActiveContext;
                    }
                }
            }

            return activeContext;
        }

        public async Task SetActiveContextAsync(Update update, string? activeContext)
        {
            if (update.Type == UpdateType.Message)
            {
                var from = update.Message!.From;
                if (from != null)
                {
                    var user = await _telegramService.GetUserByExternalIdAsync(from.Id);
                    if (user != null)
                    {
                        await _telegramService.SetActiveContextAsync(user, activeContext);
                    }
                }
            }
        }
    }
}
