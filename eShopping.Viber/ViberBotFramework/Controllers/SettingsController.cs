using eShopping.Messaging;
using eShopping.Messaging.Contracts.Distribution;
using eShopping.Viber.ViberBotFramework.Views;
using eShopping.ViberBot.Framework;
using eShopping.ViberBot.Framework.Attributes;
using eShopping.ViberBot.Framework.Contexts;
using eShopping.Viber.Models;
using eShopping.Viber.Services;
using MassTransit;

namespace eShopping.Viber.ViberBotFramework.Controllers
{
    [ViberController]
    public class SettingsController
    {
        private readonly IViberService _viberService;
        private readonly IRequestClient<GetDistributionSettingsRequest> _getDistributionSettingsRequestClient;
        private readonly IRequestClient<SetShowSalesRequest> _setShowSalesRequestClient;

        public SettingsController(
            IViberService viberService,
            IRequestClient<GetDistributionSettingsRequest> getDistributionSettingsRequestClient,
            IRequestClient<SetShowSalesRequest> setShowSalesRequestClient)
        {
            _viberService = viberService;
            _getDistributionSettingsRequestClient = getDistributionSettingsRequestClient;
            _setShowSalesRequestClient = setShowSalesRequestClient;
        }

        [TextMessage(Action = ViberAction.Settings)]
        public async Task<IViberView?> Settings(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new GetDistributionSettingsRequest(user.AccountId.Value);
                var result = await _getDistributionSettingsRequestClient.GetResponse<GetDistributionSettingsResponse>(request);
                var response = result.Message;

                var isEnabled = user.ChatSettings.IsEnabled;

                return new SettingsView(user.ExternalId, isEnabled, response.DistributionSettings);
            }

            return null;
        }

        [TextMessage(Action = ViberAction.SetIsChatEnalbed)]
        public async Task<IViberView?> SetIsChatEnabled(TextMessageContext context, bool isEnabled)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                await _viberService.SetIsChatEnabledAsync(user, isEnabled);

                var request = new GetDistributionSettingsRequest(user.AccountId.Value);
                var result = await _getDistributionSettingsRequestClient.GetResponse<GetDistributionSettingsResponse>(request);
                var response = result.Message;

                return new SettingsView(user.ExternalId, isEnabled, response.DistributionSettings);
            }

            return null;
        }

        [TextMessage(Action = ViberAction.SetShowSales)]
        public async Task<IViberView?> SetShowSales(TextMessageContext context, bool showSales)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new SetShowSalesRequest(user.AccountId.Value, showSales);
                var result = await _setShowSalesRequestClient.GetResponse<SetShowSalesResponse>(request);
                var response = result.Message;

                var isEnabled = user.ChatSettings.IsEnabled;

                return new SettingsView(user.ExternalId, isEnabled, response.DistributionSettings);
            }

            return null;
        }
    }
}
