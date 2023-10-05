using eShop.Telegram.Models;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Attributes;
using eShop.TelegramFramework.Contexts;

namespace eShop.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class SettingsController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;

        public SettingsController(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        [CallbackQuery(TelegramAction.Settings)]
        public async Task<ITelegramView?> ProcessAsync(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                return new SettingsView(context.ChatId);
            }

            return null;
        }
    }
}
