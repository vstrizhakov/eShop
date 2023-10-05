using Azure;
using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Services;
using Telegram.Bot;

namespace eShop.Telegram.MessageHandlers
{
    public class GetCurrencyRatesResponseHandler : IMessageHandler<GetCurrencyRatesResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramBotClient _botClient;
        private readonly IBotContextConverter _botContextConverter;

        public GetCurrencyRatesResponseHandler(ITelegramService telegramService, ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            _telegramService = telegramService;
            _botClient = botClient;
            _botContextConverter = botContextConverter;
        }

        public async Task HandleMessageAsync(GetCurrencyRatesResponse response)
        {
            var user = await _telegramService.GetUserByAccountIdAsync(response.AccountId);
            if (user != null)
            {
                var telegramView = new CurrencyRatesSettingsView(user.ExternalId, response.PreferredCurrency, response.CurrencyRates);
                await telegramView.ProcessAsync(_botClient, _botContextConverter);
            }
        }
    }
}
