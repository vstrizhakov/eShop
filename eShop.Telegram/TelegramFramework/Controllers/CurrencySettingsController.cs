using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Models;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Attributes;
using eShop.TelegramFramework.Contexts;

namespace eShop.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class CurrencySettingsController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;
        private readonly IRequestClient _requestClient;

        public CurrencySettingsController(ITelegramService telegramService, IRequestClient requestClient)
        {
            _telegramService = telegramService;
            _requestClient = requestClient;
        }

        [CallbackQuery(TelegramAction.CurrencySettings)]
        public async Task<ITelegramView?> ProcessAsync(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetPreferredCurrencyRequest(user.AccountId.Value);
                var response = await _requestClient.SendAsync(request);

                var view = new CurrencySettingsView(user.ExternalId, response.PreferredCurrency);
                return view;
            }

            return null;
        }
    }
}
