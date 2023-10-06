using eShop.Messaging;
using eShop.Messaging.Models.Catalog;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;

namespace eShop.Telegram.MessageHandlers
{
    public class GetCurrenciesResponseHandler : IMessageHandler<GetCurrenciesResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramViewRunner _telegramViewRunner;

        public GetCurrenciesResponseHandler(ITelegramService telegramService, ITelegramViewRunner telegramViewRunner)
        {
            _telegramService = telegramService;
            _telegramViewRunner = telegramViewRunner;
        }

        public async Task HandleMessageAsync(GetCurrenciesResponse message)
        {
            var user = await _telegramService.GetUserByAccountIdAsync(message.AccountId);
            if (user != null)
            {
                var telegramView = new PreferredCurrencySettingsView(user.ExternalId, message.Currencies);
                await _telegramViewRunner.RunAsync(telegramView);
            }
        }
    }
}
