using eShop.Messaging;
using eShop.Messaging.Models.Catalog;
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
    public class CurrencyController
    {
        private readonly IViberService _viberService;
        private readonly IRequestClient _requestClient;

        public CurrencyController(IViberService viberService, IRequestClient requestClient)
        {
            _viberService = viberService;
            _requestClient = requestClient;
        }

        [TextMessage(Action = ViberContext.CurrencySettings)]
        public async Task<IViberView?> CurrencySettings(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new GetPreferredCurrencyRequest(user.AccountId.Value);
                var response = await _requestClient.SendAsync(request);

                return new CurrencySettingsView(user.ExternalId, response.PreferredCurrency);
            }

            return null;
        }

        [TextMessage(Action = ViberContext.PreferredCurrencySettings)]
        public async Task<IViberView?> PreferredCurrencySettings(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new GetCurrenciesRequest(user.AccountId.Value);
                var response = await _requestClient.SendAsync(request);

                return new PreferredCurrencySettingsView(user.ExternalId, response.Currencies);
            }

            return null;
        }

        [TextMessage(Action = ViberContext.SetPreferredCurrency)]
        public async Task<IViberView?> SetPreferredCurrency(TextMessageContext context, Guid currencyId)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new SetPreferredCurrencyRequest(user.AccountId.Value, currencyId);
                var response = await _requestClient.SendAsync(request);

                return new CurrencySettingsView(user.ExternalId, response.PreferredCurrency);
            }

            return null;
        }
    }
}
