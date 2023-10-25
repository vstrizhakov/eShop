using eShop.Messaging;
using eShop.Messaging.Models.Distribution.ShopSettings;
using eShop.Viber.Models;
using eShop.Viber.Services;
using eShop.Viber.ViberBotFramework.Views;
using eShop.ViberBot.Framework;
using eShop.ViberBot.Framework.Attributes;
using eShop.ViberBot.Framework.Contexts;

namespace eShop.Viber.ViberBotFramework.Controllers
{
    [ViberController]
    public class ShopSettingsController
    {
        private readonly IViberService _viberService;
        private readonly IRequestClient _requestClient;

        public ShopSettingsController(IViberService viberService, IRequestClient requestClient)
        {
            _viberService = viberService;
            _requestClient = requestClient;
        }

        [TextMessage(Action = ViberContext.ShopSettings)]
        public async Task<IViberView?> ShopSettings(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new GetShopSettingsRequest(user.AccountId.Value);
                var response = await _requestClient.SendAsync(request);

                return new ShopSettingsView(user.ExternalId, response.ShopSettings);
            }

            return null;
        }

        [TextMessage(Action = ViberContext.SetShopSettingsFilter)]
        public async Task<IViberView?> SetShopSettingsFilter(TextMessageContext context, bool filter)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new SetShopSettingsFilterRequest(user.AccountId.Value, filter);
                var response = await _requestClient.SendAsync(request);

                return new ShopSettingsView(user.ExternalId, response.ShopSettings);
            }

            return null;
        }

        [TextMessage(Action = ViberContext.ShopSettingsShops)]
        public async Task<IViberView?> ShopSettingsShops(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new GetShopSettingsShopsRequest(user.AccountId.Value);
                var response = await _requestClient.SendAsync(request);

                return new ShopSettingsShopsView(user.ExternalId, response.Shops);
            }

            return null;
        }

        [TextMessage(Action = ViberContext.SetShopSettingsShopState)]
        public async Task<IViberView?> SetShopSettingsShopState(TextMessageContext context, Guid shopId, bool shopEnabled)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new SetShopSettingsShopStateRequest(user.AccountId.Value, shopId, shopEnabled);
                var response = await _requestClient.SendAsync(request);

                return new ShopSettingsShopsView(user.ExternalId, response.Shops);
            }

            return null;
        }
    }
}
