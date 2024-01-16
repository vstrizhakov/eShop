using eShopping.Telegram.TelegramFramework.Views;
using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Attributes;
using eShopping.TelegramFramework.Contexts;
using eShopping.Telegram.Models;
using eShopping.Telegram.Services;

namespace eShopping.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class WelcomeController
    {
        private readonly ITelegramService _telegramService;

        public WelcomeController(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        [CallbackQuery(TelegramAction.Home)]
        public async Task<ITelegramView?> ProcessAsync(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                return new WelcomeView(context.ChatId, context.MessageId);
            }

            return null;
        }
    }
}
