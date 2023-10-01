using eShop.Telegram.Inner.Contexts;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Models;
using eShop.Telegram.Services;

namespace eShop.Telegram.Inner.Controllers
{
    [TelegramController(TelegramAction.Settings, Context = TelegramContext.CallbackQuery)]
    public class SettingsController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;

        public SettingsController(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

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
