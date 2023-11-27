using eShop.Messaging.Contracts.Catalog;
using eShop.Messaging.Contracts.Distribution;
using eShop.Telegram.Models;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Attributes;
using eShop.TelegramFramework.Contexts;
using MassTransit;

namespace eShop.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class PreferredCurrencySettingsController
    {
        private readonly ITelegramService _telegramService;
        private readonly IRequestClient<GetCurrenciesRequest> _getCurrenciesRequestClient;
        private readonly IRequestClient<SetPreferredCurrencyRequest> _setPreferredCurrencyRequestClient;

        public PreferredCurrencySettingsController(ITelegramService telegramService, IRequestClient<GetCurrenciesRequest> requestClient, IRequestClient<SetPreferredCurrencyRequest> setPreferredCurrencyRequestClient)
        {
            _telegramService = telegramService;
            _getCurrenciesRequestClient = requestClient;
            _setPreferredCurrencyRequestClient = setPreferredCurrencyRequestClient;
        }

        [CallbackQuery(TelegramAction.PreferredCurrencySettings)]
        public async Task<ITelegramView?> GetPreferredCurrency(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetCurrenciesRequest(user.AccountId.Value);
                var response = await _getCurrenciesRequestClient.GetResponse<GetCurrenciesResponse>(request);
                
                var view = new PreferredCurrencySettingsView(user.ExternalId, context.MessageId, response.Message.Currencies);
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
                var response = await _setPreferredCurrencyRequestClient.GetResponse<SetPreferredCurrencyResponse>(request);

                var view = new CurrencySettingsView(user.ExternalId, context.MessageId, response.Message.PreferredCurrency);
                return view;
            }

            return null;
        }
    }
}
