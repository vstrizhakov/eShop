using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models.Catalog;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Services;
using Telegram.Bot;

namespace eShop.Telegram.MessageHandlers
{
    public class GetCurrenciesResponseHandler : IMessageHandler<GetCurrenciesResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramBotClient _botClient;
        private readonly IBotContextConverter _botContextConverter;

        public GetCurrenciesResponseHandler(ITelegramService telegramService, ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            _telegramService = telegramService;
            _botClient = botClient;
            _botContextConverter = botContextConverter;
        }

        public async Task HandleMessageAsync(GetCurrenciesResponse message)
        {
            var user = await _telegramService.GetUserByAccountIdAsync(message.AccountId);
            if (user != null)
            {
                var telegramView = new PreferredCurrencySettingsView(user.ExternalId, message.Currencies);
                await telegramView.ProcessAsync(_botClient, _botContextConverter);
            }
        }
    }
}
