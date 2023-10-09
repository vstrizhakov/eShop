using eShop.Messaging;
using eShop.Messaging.Models.Distribution.ShopSettings;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;

namespace eShop.Telegram.MessageHandlers
{
    public class SetShopSettingsFilterResponseHandler : IMessageHandler<SetShopSettingsFilterResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramViewRunner _telegramViewRunner;

        public SetShopSettingsFilterResponseHandler(ITelegramService telegramService, ITelegramViewRunner telegramViewRunner)
        {
            _telegramService = telegramService;
            _telegramViewRunner = telegramViewRunner;
        }

        public async Task HandleMessageAsync(SetShopSettingsFilterResponse message)
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
