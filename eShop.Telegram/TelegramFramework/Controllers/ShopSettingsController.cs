using eShop.Messaging;
using eShop.Messaging.Models.Distribution.ShopSettings;
using eShop.Telegram.Models;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Attributes;
using eShop.TelegramFramework.Contexts;

namespace eShop.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class ShopSettingsController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;
        private readonly IRequestClient _requestClient;

        public ShopSettingsController(ITelegramService telegramService, IRequestClient requestClient)
        {
            _telegramService = telegramService;
            _requestClient = requestClient;
        }

        [CallbackQuery(TelegramAction.ShopSettings)]
        public async Task<ITelegramView?> GetShopSettings(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetShopSettingsRequest(user.AccountId.Value);
                var response = await _requestClient.SendAsync(request);

                var view = new ShopSettingsView(user.ExternalId, response.ShopSettings);
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
                var response = await _requestClient.SendAsync(request);

                var view = new ShopSettingsView(user.ExternalId, response.ShopSettings);
                return view;
            }

            return null;
        }

        [CallbackQuery(TelegramAction.ShopSettingsShops)]
        public async Task<ITelegramView?> GetShopSettingsShops(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetShopSettingsShopsRequest(user.AccountId.Value);
                var response = await _requestClient.SendAsync(request);

                var view = new ShopSettingsShopsView(user.ExternalId, response.Shops);
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
                var response = await _requestClient.SendAsync(request);

                var view = new ShopSettingsShopsView(user.ExternalId, response.Shops);
                return view;
            }

            return null;
        }
    }
}
