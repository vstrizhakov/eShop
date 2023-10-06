using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using Telegram.Bot;

namespace eShop.Telegram.MessageHandlers
{
    public class GetPreferredCurrencyResponseHandler : IMessageHandler<GetPreferredCurrencyResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramViewRunner _telegramViewRunner;

        public GetPreferredCurrencyResponseHandler(ITelegramService telegramService, ITelegramViewRunner telegramViewRunner)
        {
            _telegramService = telegramService;
            _telegramViewRunner = telegramViewRunner;
        }

        public async Task HandleMessageAsync(GetPreferredCurrencyResponse response)
        {
            var user = await _telegramService.GetUserByAccountIdAsync(response.AccountId);
            if (user != null)
            {
                var telegramView = new CurrencySettingsView(user.ExternalId, response.PreferredCurrency);
                await _telegramViewRunner.RunAsync(telegramView);
            }
        }
    }
}
