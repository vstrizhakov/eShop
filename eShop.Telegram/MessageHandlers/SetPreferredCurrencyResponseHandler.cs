using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using Telegram.Bot;

namespace eShop.Telegram.MessageHandlers
{
    public class SetPreferredCurrencyResponseHandler : IMessageHandler<SetPreferredCurrencyResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramViewRunner _telegramViewRunner;

        public SetPreferredCurrencyResponseHandler(ITelegramService telegramService, ITelegramViewRunner telegramViewRunner)
        {
            _telegramService = telegramService;
            _telegramViewRunner = telegramViewRunner;
        }

        public async Task HandleMessageAsync(SetPreferredCurrencyResponse message)
        {
            var user = await _telegramService.GetUserByAccountIdAsync(message.AccountId);
            if (user != null)
            {
                var telegramView = new CurrencySettingsView(user.ExternalId, message.PreferredCurrency);
                await _telegramViewRunner.RunAsync(telegramView);
            }
        }
    }
}
