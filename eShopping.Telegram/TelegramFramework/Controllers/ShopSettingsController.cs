using eShopping.Messaging.Contracts.Distribution.ShopSettings;
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
    public class ShopSettingsController
    {
        private readonly ITelegramService _telegramService;
        private readonly IRequestClient<GetShopSettingsRequest> _getShopSettingsRequestClient;
        private readonly IRequestClient<SetShopSettingsFilterRequest> _setShopSettingsFilterRequestClient;
        private readonly IRequestClient<GetShopSettingsShopsRequest> _getShopSettingsShopsRequestClient;
        private readonly IRequestClient<SetShopSettingsShopStateRequest> _setShopSettingsShopStateRequestClient;

        public ShopSettingsController(
            ITelegramService telegramService,
            IRequestClient<GetShopSettingsRequest> getShopSettingsRequestClient,
            IRequestClient<SetShopSettingsFilterRequest> setShopSettingsFilterRequestClient,
            IRequestClient<GetShopSettingsShopsRequest> getShopSettingsShopsRequestClient,
            IRequestClient<SetShopSettingsShopStateRequest> setShopSettingsShopStateRequestClient)
        {
            _telegramService = telegramService;
            _getShopSettingsRequestClient = getShopSettingsRequestClient;
            _setShopSettingsFilterRequestClient = setShopSettingsFilterRequestClient;
            _getShopSettingsShopsRequestClient = getShopSettingsShopsRequestClient;
            _setShopSettingsShopStateRequestClient = setShopSettingsShopStateRequestClient;
        }

        [CallbackQuery(TelegramAction.ShopSettings)]
        public async Task<ITelegramView?> GetShopSettings(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetShopSettingsRequest(user.AccountId.Value);
                var result = await _getShopSettingsRequestClient.GetResponse<GetShopSettingsResponse>(request);
                var response = result.Message;

                var view = new ShopSettingsView(user.ExternalId, context.MessageId, response.ShopSettings);
                return view;
            }

            return null;
        }

        [CallbackQuery(TelegramAction.SetShopSettingsFilter)]
        public async Task<ITelegramView?> SetShopSettingsFilter(CallbackQueryContext context, bool filter)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new SetShopSettingsFilterRequest(user.AccountId.Value, filter);
                var result = await _setShopSettingsFilterRequestClient.GetResponse<SetShopSettingsFilterResponse>(request);
                var response = result.Message;

                var view = new ShopSettingsView(user.ExternalId, context.MessageId, response.ShopSettings);
                return view;
            }

            return null;
        }

        [CallbackQuery(TelegramAction.ShopSettingsShops)]
        public async Task<ITelegramView?> GetShopSettingsShops(CallbackQueryContext context, int page = 0)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetShopSettingsShopsRequest(user.AccountId.Value);
                var result = await _getShopSettingsShopsRequestClient.GetResponse<GetShopSettingsShopsResponse>(request);
                var response = result.Message;

                var view = new ShopSettingsShopsView(user.ExternalId, context.MessageId, response.Shops, page);
                return view;
            }

            return null;
        }

        [CallbackQuery(TelegramAction.SetShopSettingsShopState)]
        public async Task<ITelegramView?> SetShopSettingsShopState(CallbackQueryContext context, Guid shopId, bool isEnabled)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new SetShopSettingsShopStateRequest(user.AccountId.Value, shopId, isEnabled);
                var result = await _setShopSettingsShopStateRequestClient.GetResponse<SetShopSettingsShopStateResponse>(request);
                var response = result.Message;

                var view = new ShopSettingsShopsView(user.ExternalId, context.MessageId, response.Shops);
                return view;
            }

            return null;
        }
    }
}
