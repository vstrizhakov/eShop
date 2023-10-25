using eShop.Messaging;
using eShop.Messaging.Models.Catalog;
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
    public class PreferredCurrencySettingsController
    {
        private readonly ITelegramService _telegramService;
        private readonly IRequestClient _requestClient;

        public PreferredCurrencySettingsController(ITelegramService telegramService, IRequestClient requestClient)
        {
            _telegramService = telegramService;
            _requestClient = requestClient;
        }

        [CallbackQuery(TelegramAction.PreferredCurrencySettings)]
        public async Task<ITelegramView?> GetPreferredCurrency(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetCurrenciesRequest(user.AccountId.Value);
                var response = await _requestClient.SendAsync(request);

                var view = new PreferredCurrencySettingsView(user.ExternalId, context.MessageId, response.Currencies);
                return view;
            }

            return null;
        }

        [CallbackQuery(TelegramAction.SetPreferredCurrency)]
        public async Task<ITelegramView?> SetPreferredCurrency(CallbackQueryContext context, Guid currencyId)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new SetPreferredCurrencyRequest(user.AccountId.Value, currencyId);
                var response = await _requestClient.SendAsync(request);

                var view = new CurrencySettingsView(user.ExternalId, context.MessageId, response.PreferredCurrency);
                return view;
            }

            return null;
        }
    }
}
