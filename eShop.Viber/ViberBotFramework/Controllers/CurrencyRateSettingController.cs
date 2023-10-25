using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Viber.Models;
using eShop.Viber.Services;
using eShop.Viber.ViberBotFramework.Views;
using eShop.ViberBot.Framework;
using eShop.ViberBot.Framework.Attributes;
using eShop.ViberBot.Framework.Contexts;

namespace eShop.Viber.ViberBotFramework.Controllers
{
    [ViberController]
    public class CurrencyRateSettingController
    {
        private readonly IViberService _viberService;
        private readonly IRequestClient _requestClient;
        private readonly IBotContextConverter _botContextConverter;

        public CurrencyRateSettingController(IViberService viberService, IRequestClient requestClient, IBotContextConverter botContextConverter)
        {
            _viberService = viberService;
            _requestClient = requestClient;
            _botContextConverter = botContextConverter;
        }

        [TextMessage(Action = ViberContext.CurrencyRateSettings)]
        public async Task<IViberView?> CurrencyRateSettings(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new GetCurrencyRatesRequest(user.AccountId.Value);
                var response = await _requestClient.SendAsync(request);

                return new CurrencyRateSettingsView(user.ExternalId, response.PreferredCurrency, response.CurrencyRates);
            }

            return null;
        }

        [TextMessage(Action = ViberContext.SetCurrencyRate)]
        public async Task<IViberView?> GetCurrencyRate(TextMessageContext context, Guid currencyId)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new GetCurrencyRateRequest(user.AccountId.Value, currencyId);
                var response = await _requestClient.SendAsync(request);

                var activeContext = _botContextConverter.Serialize(ViberContext.SetCurrencyRate, currencyId.ToString());
                await _viberService.SetActiveContextAsync(user, activeContext);

                return new SetCurrencyRateView(user.ExternalId, response.PreferredCurrency, response.CurrencyRate);
            }

            return null;
        }

        [TextMessage(ActiveAction = ViberContext.SetCurrencyRate)]
        public async Task<IViberView?> SetCurrencyRate(TextMessageContext context, Guid currencyId)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                if (double.TryParse(context.Text, out var rate))
                {
                    var request = new SetCurrencyRateRequest(user.AccountId.Value, currencyId, rate);
                    var response = await _requestClient.SendAsync(request);

                    await _viberService.SetActiveContextAsync(user, null);

                    return new CurrencyRateSettingsView(user.ExternalId, response.PreferredCurrency, response.CurrencyRates);
                }
            }

            return null;
        }
    }
}
