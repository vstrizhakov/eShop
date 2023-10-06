using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using Telegram.Bot;

namespace eShop.Telegram.MessageHandlers
{
    public class SetCurrencyRateResponseHandler : IMessageHandler<SetCurrencyRateResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramViewRunner _telegramViewRunner;

        public SetCurrencyRateResponseHandler(ITelegramService telegramService, ITelegramViewRunner telegramViewRunner)
        {
            _telegramService = telegramService;
            _telegramViewRunner = telegramViewRunner;
        }

        public async Task HandleMessageAsync(SetCurrencyRateResponse message)
        {
            var user = await _telegramService.GetUserByAccountIdAsync(message.AccountId);
            if (user != null)
            {
                var telegramView = new CurrencyRatesSettingsView(user.ExternalId, message.PreferredCurrency, message.CurrencyRates);
                await _telegramViewRunner.RunAsync(telegramView);
            }
        }
    }
}
