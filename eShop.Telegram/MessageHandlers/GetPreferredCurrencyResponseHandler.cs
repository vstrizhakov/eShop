using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using Telegram.Bot;

namespace eShop.Telegram.MessageHandlers
{
    public class GetPreferredCurrencyResponseHandler : IMessageHandler<GetPreferredCurrencyResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramBotClient _botClient;
        private readonly IBotContextConverter _botContextConverter;

        public GetPreferredCurrencyResponseHandler(ITelegramService telegramService, ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            _telegramService = telegramService;
            _botClient = botClient;
            _botContextConverter = botContextConverter;
        }

        public async Task HandleMessageAsync(GetPreferredCurrencyResponse response)
        {
            var user = await _telegramService.GetUserByAccountIdAsync(response.AccountId);
            if (user != null)
            {
                var telegramView = new CurrencySettingsView(user.ExternalId, response.PreferredCurrency);
                await telegramView.ProcessAsync(_botClient, _botContextConverter);
            }
        }
    }
}
