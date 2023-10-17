using eShop.Bots.Common;
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
    public class CurrencyRateSettings : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;
        private readonly IRequestClient _requestClient;
        private readonly IBotContextConverter _botContextConverter;

        public CurrencyRateSettings(ITelegramService telegramService, IRequestClient request, IBotContextConverter botContextConverter)
        {
            _telegramService = telegramService;
            _requestClient = request;
            _botContextConverter = botContextConverter;
        }

        [CallbackQuery(TelegramAction.CurrencyRatesSettings)]
        public async Task<ITelegramView?> GetCurrencyRates(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetCurrencyRatesRequest(user.AccountId.Value);
                var response = await _requestClient.SendAsync(request);

                var view = new CurrencyRatesSettingsView(user.ExternalId, context.MessageId, response.PreferredCurrency, response.CurrencyRates);
                return view;
            }

            return null;
        }

        [CallbackQuery(TelegramAction.SetCurrencyRate)]
        public async Task<ITelegramView?> GetCurrencyRate(CallbackQueryContext context, Guid currencyId)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetCurrencyRateRequest(user.AccountId.Value, currencyId);
                var response = await _requestClient.SendAsync(request);

                if (response.IsSucceeded)
                {
                    var currencyRate = response.CurrencyRate!;

                    var activeContext = _botContextConverter.Serialize(TelegramAction.SetCurrencyRate, currencyRate.Currency.Id.ToString());
                    await _telegramService.SetActiveContextAsync(user, activeContext);

                    var view = new SetCurrencyRateView(user.ExternalId, response.PreferredCurrency!, currencyRate);
                    return view;
                }
            }

            return null;
        }

        [TextMessage(Action = TelegramAction.SetCurrencyRate)]
        public async Task<ITelegramView?> SetCurrencyRate(TextMessageContext context, Guid currencyId)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                if (double.TryParse(context.Text.Replace(',', '.'), out var rate))
                {
                    await _telegramService.SetActiveContextAsync(user, null);

                    var request = new SetCurrencyRateRequest(user.AccountId.Value, currencyId, rate);
                    var response = await _requestClient.SendAsync(request);

                    var view = new CurrencyRatesSettingsView(user.ExternalId, null, response.PreferredCurrency, response.CurrencyRates);
                    return view;
                }
                else
                {
                    // TODO: handle
                }
            }

            return null;
        }
    }
}
