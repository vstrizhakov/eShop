using eShop.Bots.Common;
using eShop.Messaging.Contracts.Distribution;
using eShop.Viber.Models;
using eShop.Viber.Services;
using eShop.Viber.ViberBotFramework.Views;
using eShop.ViberBot.Framework;
using eShop.ViberBot.Framework.Attributes;
using eShop.ViberBot.Framework.Contexts;
using MassTransit;

namespace eShop.Viber.ViberBotFramework.Controllers
{
    [ViberController]
    public class CurrencyRateSettingController
    {
        private readonly IViberService _viberService;
        private readonly IRequestClient<GetCurrencyRatesRequest> _getCurrencyRatesRequestClient;
        private readonly IRequestClient<GetCurrencyRateRequest> _getCurrencyRateRequestClient;
        private readonly IRequestClient<SetCurrencyRateRequest> _setCurrencyRateRequestClient;
        private readonly IBotContextConverter _botContextConverter;

        public CurrencyRateSettingController(
            IViberService viberService,
            IBotContextConverter botContextConverter,
            IRequestClient<GetCurrencyRatesRequest> getCurrencyRatesRequestClient,
            IRequestClient<GetCurrencyRateRequest> getCurrencyRateRequestClient,
            IRequestClient<SetCurrencyRateRequest> setCurrencyRateRequestClient)
        {
            _viberService = viberService;
            _botContextConverter = botContextConverter;
            _getCurrencyRatesRequestClient = getCurrencyRatesRequestClient;
            _getCurrencyRateRequestClient = getCurrencyRateRequestClient;
            _setCurrencyRateRequestClient = setCurrencyRateRequestClient;
        }

        [TextMessage(Action = ViberAction.CurrencyRateSettings)]
        public async Task<IViberView?> CurrencyRateSettings(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new GetCurrencyRatesRequest(user.AccountId.Value);
                var result = await _getCurrencyRatesRequestClient.GetResponse<GetCurrencyRatesResponse>(request);
                var response = result.Message;

                return new CurrencyRateSettingsView(user.ExternalId, response.PreferredCurrency, response.CurrencyRates);
            }

            return null;
        }

        [TextMessage(Action = ViberAction.SetCurrencyRate)]
        public async Task<IViberView?> GetCurrencyRate(TextMessageContext context, Guid currencyId)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new GetCurrencyRateRequest(user.AccountId.Value, currencyId);
                var result = await _getCurrencyRateRequestClient.GetResponse<GetCurrencyRateResponse>(request);
                var response = result.Message;
                if (response.Succeeded)
                {
                    var activeContext = _botContextConverter.Serialize(ViberAction.SetCurrencyRate, currencyId.ToString());
                    await _viberService.SetActiveContextAsync(user, activeContext);

                    return new SetCurrencyRateView(user.ExternalId, response.PreferredCurrency!, response.CurrencyRate!);
                }
            }

            return null;
        }

        [TextMessage(ActiveAction = ViberAction.SetCurrencyRate)]
        public async Task<IViberView?> SetCurrencyRate(TextMessageContext context, Guid currencyId)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                if (double.TryParse(context.Text, out var rate))
                {
                    var request = new SetCurrencyRateRequest(user.AccountId.Value, currencyId, rate);
                    var result = await _setCurrencyRateRequestClient.GetResponse<SetCurrencyRateResponse>(request);
                    var response = result.Message;

                    await _viberService.SetActiveContextAsync(user, null);

                    return new CurrencyRateSettingsView(user.ExternalId, response.PreferredCurrency, response.CurrencyRates);
                }
            }

            return null;
        }
    }
}
