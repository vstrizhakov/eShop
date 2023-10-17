using eShop.Telegram.Models;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Attributes;
using eShop.TelegramFramework.Contexts;

namespace eShop.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class WelcomeController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;

        public WelcomeController(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        [TextMessage(Command = "/start")]
        public async Task<ITelegramView?> ProcessAsync(TextMessageContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                return new WelcomeView(context.ChatId, null);
            }

            return null;
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
