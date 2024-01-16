using eShopping.Bots.Common;
using eShopping.Messaging;
using eShopping.Messaging.Contracts.Distribution;
using eShopping.Telegram.TelegramFramework.Views;
using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Attributes;
using eShopping.TelegramFramework.Contexts;
using eShopping.Telegram.Models;
using eShopping.Telegram.Services;
using MassTransit;

namespace eShopping.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class CurrencyRateSettingsController
    {
        private readonly ITelegramService _telegramService;
        private readonly IRequestClient<GetCurrencyRatesRequest> _getCurrencyRatesRequestClient;
        private readonly IRequestClient<GetCurrencyRateRequest> _getCurrencyRateRequestClient;
        private readonly IRequestClient<SetCurrencyRateRequest> _setCurrencyRateRequestClient;
        private readonly IBotContextConverter _botContextConverter;

        public CurrencyRateSettingsController(
            ITelegramService telegramService,
            IBotContextConverter botContextConverter,
            IRequestClient<GetCurrencyRatesRequest> getCurrencyRatesRequestClient,
            IRequestClient<GetCurrencyRateRequest> getCurrencyRateRequestClient,
            IRequestClient<SetCurrencyRateRequest> setCurrencyRateRequestClient)
        {
            _telegramService = telegramService;
            _botContextConverter = botContextConverter;
            _getCurrencyRatesRequestClient = getCurrencyRatesRequestClient;
            _getCurrencyRateRequestClient = getCurrencyRateRequestClient;
            _setCurrencyRateRequestClient = setCurrencyRateRequestClient;
        }

        [CallbackQuery(TelegramAction.CurrencyRatesSettings)]
        public async Task<ITelegramView?> GetCurrencyRates(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetCurrencyRatesRequest(user.AccountId.Value);
                var result = await _getCurrencyRatesRequestClient.GetResponse<GetCurrencyRatesResponse>(request);
                var response = result.Message;

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
                var result = await _getCurrencyRateRequestClient.GetResponse<GetCurrencyRateResponse>(request);
                var response = result.Message;

                if (response.Succeeded)
                {
                    var activeContext = _botContextConverter.Serialize(TelegramAction.SetCurrencyRate, currencyId.ToString());
                    await _telegramService.SetActiveContextAsync(user, activeContext);

                    var view = new SetCurrencyRateView(user.ExternalId, response.PreferredCurrency!, response.CurrencyRate!);
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
                if (decimal.TryParse(context.Text.Replace(',', '.'), out var rate))
                {
                    await _telegramService.SetActiveContextAsync(user, null);

                    var request = new SetCurrencyRateRequest(user.AccountId.Value, currencyId, rate);
                    var result = await _setCurrencyRateRequestClient.GetResponse<SetCurrencyRateResponse>(request);
                    var response = result.Message;

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
