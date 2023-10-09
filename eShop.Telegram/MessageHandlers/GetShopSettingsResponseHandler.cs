using eShop.Messaging;
using eShop.Messaging.Models.Catalog;
using eShop.Messaging.Models.Distribution.ShopSettings;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;

namespace eShop.Telegram.MessageHandlers
{
    public class GetShopSettingsResponseHandler : IMessageHandler<GetShopSettingsResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramViewRunner _telegramViewRunner;

        public GetShopSettingsResponseHandler(ITelegramService telegramService, ITelegramViewRunner telegramViewRunner)
        {
            _telegramService = telegramService;
            _telegramViewRunner = telegramViewRunner;
        }

        public async Task HandleMessageAsync(GetShopSettingsResponse message)
        {
            var user = await _telegramService.GetUserByAccountIdAsync(message.AccountId);
            if (user != null)
            {
                var telegramView = new ShopSettingsView(user.ExternalId, message.ShopSettings);
                await _telegramViewRunner.RunAsync(telegramView);
            }
        }
    }
}
