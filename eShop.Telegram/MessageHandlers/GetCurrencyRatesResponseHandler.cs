using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;

namespace eShop.Telegram.MessageHandlers
{
    public class GetCurrencyRatesResponseHandler : IMessageHandler<GetCurrencyRatesResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramViewRunner _telegramViewRunner;

        public GetCurrencyRatesResponseHandler(ITelegramService telegramService, ITelegramViewRunner telegramViewRunner)
        {
            _telegramService = telegramService;
            _telegramViewRunner = telegramViewRunner;
        }

        public async Task HandleMessageAsync(GetCurrencyRatesResponse response)
        {
            var user = await _telegramService.GetUserByAccountIdAsync(response.AccountId);
            if (user != null)
            {
                var telegramView = new CurrencyRatesSettingsView(user.ExternalId, response.PreferredCurrency, response.CurrencyRates);
                await _telegramViewRunner.RunAsync(telegramView);
            }
        }
    }
}
