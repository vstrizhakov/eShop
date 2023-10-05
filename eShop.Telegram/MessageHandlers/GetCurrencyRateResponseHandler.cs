using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Models;
using eShop.Telegram.Services;
using Telegram.Bot;

namespace eShop.Telegram.MessageHandlers
{
    public class GetCurrencyRateResponseHandler : IMessageHandler<GetCurrencyRateResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramBotClient _botClient;
        private readonly IBotContextConverter _botContextConverter;

        public GetCurrencyRateResponseHandler(ITelegramService telegramService, ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            _telegramService = telegramService;
            _botClient = botClient;
            _botContextConverter = botContextConverter;
        }

        public async Task HandleMessageAsync(GetCurrencyRateResponse message)
        {
            if (message.IsSucceeded)
            {
                var user = await _telegramService.GetUserByAccountIdAsync(message.AccountId);
                if (user != null)
                {
                    var currencyRate = message.CurrencyRate!;

                    var activeContext = _botContextConverter.Serialize(TelegramAction.SetCurrencyRate, currencyRate.Currency.Id.ToString());
                    await _telegramService.SetActiveContextAsync(user, activeContext);

                    var telegramView = new SetCurrencyRateView(user.ExternalId, message.PreferredCurrency!, currencyRate);
                    await telegramView.ProcessAsync(_botClient, _botContextConverter);
                }
            }
        }
    }
}
