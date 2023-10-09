using eShop.Messaging;
using eShop.Messaging.Models.Distribution.ShopSettings;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;

namespace eShop.Telegram.MessageHandlers
{
    public class SetShopSettingsShopStateResponseHandler : IMessageHandler<SetShopSettingsShopStateResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramViewRunner _telegramViewRunner;

        public SetShopSettingsShopStateResponseHandler(ITelegramService telegramService, ITelegramViewRunner telegramViewRunner)
        {
            _telegramService = telegramService;
            _telegramViewRunner = telegramViewRunner;
        }

        public async Task HandleMessageAsync(SetShopSettingsShopStateResponse message)
        {
            var user = await _telegramService.GetUserByAccountIdAsync(message.AccountId);
            if (user != null)
            {
                var telegramView = new ShopSettingsShopsView(user.ExternalId, message.Shops);
                await _telegramViewRunner.RunAsync(telegramView);
            }
        }
    }
}
