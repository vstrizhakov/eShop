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
    public class SettingsController
    {
        private readonly IViberService _viberService;
        private readonly IRequestClient _requestClient;

        public SettingsController(IViberService viberService, IRequestClient requestClient)
        {
            _viberService = viberService;
            _requestClient = requestClient;
        }

        [TextMessage(Action = ViberContext.Settings)]
        public async Task<IViberView?> Settings(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new GetDistributionSettingsRequest(user.AccountId.Value);
                var response = await _requestClient.SendAsync(request);

                var isEnabled = user.ChatSettings.IsEnabled;

                return new SettingsView(user.ExternalId, isEnabled, response.DistributionSettings);
            }

            return null;
        }

        [TextMessage(Action = ViberContext.SetIsChatEnalbed)]
        public async Task<IViberView?> SetIsChatEnabled(TextMessageContext context, bool isEnabled)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                await _viberService.SetIsChatEnabledAsync(user, isEnabled);

                var request = new GetDistributionSettingsRequest(user.AccountId.Value);
                var response = await _requestClient.SendAsync(request);

                return new SettingsView(user.ExternalId, isEnabled, response.DistributionSettings);
            }

            return null;
        }

        [TextMessage(Action = ViberContext.SetShowSales)]
        public async Task<IViberView?> SetShowSales(TextMessageContext context, bool showSales)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new SetShowSalesRequest(user.AccountId.Value, showSales);
                var response = await _requestClient.SendAsync(request);

                var isEnabled = user.ChatSettings.IsEnabled;

                return new SettingsView(user.ExternalId, isEnabled, response.DistributionSettings);
            }

            return null;
        }
    }
}
