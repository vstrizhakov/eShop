using eShop.Messaging.Contracts.Distribution.ShopSettings;
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
    public class ShopSettingsController
    {
        private readonly IViberService _viberService;
        private readonly IRequestClient<GetShopSettingsRequest> _getShopSettingsRequestClient;
        private readonly IRequestClient<SetShopSettingsFilterRequest> _setShopSettingsFilterRequestClient;
        private readonly IRequestClient<GetShopSettingsShopsRequest> _getShopSettingsShopsRequestClient;
        private readonly IRequestClient<SetShopSettingsShopStateRequest> _setShopSettingsShopStateRequestClient;

        public ShopSettingsController(
            IViberService viberService,
            IRequestClient<GetShopSettingsRequest> getShopSettingsRequestClient,
            IRequestClient<SetShopSettingsFilterRequest> setShopSettingsFilterRequestClient,
            IRequestClient<GetShopSettingsShopsRequest> getShopSettingsShopsRequestClient,
            IRequestClient<SetShopSettingsShopStateRequest> setShopSettingsShopStateRequestClient)
        {
            _viberService = viberService;
            _getShopSettingsRequestClient = getShopSettingsRequestClient;
            _setShopSettingsFilterRequestClient = setShopSettingsFilterRequestClient;
            _getShopSettingsShopsRequestClient = getShopSettingsShopsRequestClient;
            _setShopSettingsShopStateRequestClient = setShopSettingsShopStateRequestClient;
        }

        [TextMessage(Action = ViberAction.ShopSettings)]
        public async Task<IViberView?> ShopSettings(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new GetShopSettingsRequest(user.AccountId.Value);
                var result = await _getShopSettingsRequestClient.GetResponse<GetShopSettingsResponse>(request);
                var response = result.Message;

                return new ShopSettingsView(user.ExternalId, response.ShopSettings);
            }

            return null;
        }

        [TextMessage(Action = ViberAction.SetShopSettingsFilter)]
        public async Task<IViberView?> SetShopSettingsFilter(TextMessageContext context, bool filter)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new SetShopSettingsFilterRequest(user.AccountId.Value, filter);
                var result = await _setShopSettingsFilterRequestClient.GetResponse<SetShopSettingsFilterResponse>(request);
                var response = result.Message;

                return new ShopSettingsView(user.ExternalId, response.ShopSettings);
            }

            return null;
        }

        [TextMessage(Action = ViberAction.ShopSettingsShops)]
        public async Task<IViberView?> ShopSettingsShops(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new GetShopSettingsShopsRequest(user.AccountId.Value);
                var result = await _getShopSettingsShopsRequestClient.GetResponse<GetShopSettingsShopsResponse>(request);
                var response = result.Message;

                return new ShopSettingsShopsView(user.ExternalId, response.Shops);
            }

            return null;
        }

        [TextMessage(Action = ViberAction.SetShopSettingsShopState)]
        public async Task<IViberView?> SetShopSettingsShopState(TextMessageContext context, Guid shopId, bool shopEnabled)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new SetShopSettingsShopStateRequest(user.AccountId.Value, shopId, shopEnabled);
                var result = await _setShopSettingsShopStateRequestClient.GetResponse<SetShopSettingsShopStateResponse>(request);
                var response = result.Message;

                return new ShopSettingsShopsView(user.ExternalId, response.Shops);
            }

            return null;
        }
    }
}
