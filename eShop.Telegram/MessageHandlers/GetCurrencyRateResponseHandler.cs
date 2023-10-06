using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Models;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using Telegram.Bot;

namespace eShop.Telegram.MessageHandlers
{
    public class GetCurrencyRateResponseHandler : IMessageHandler<GetCurrencyRateResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly IBotContextConverter _botContextConverter;
        private readonly ITelegramViewRunner _telegramViewRunner;

        public GetCurrencyRateResponseHandler(ITelegramService telegramService, IBotContextConverter botContextConverter, ITelegramViewRunner telegramViewRunner)
        {
            _telegramService = telegramService;
            _botContextConverter = botContextConverter;
            _telegramViewRunner = telegramViewRunner;
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
                    await _telegramViewRunner.RunAsync(telegramView);
                }
            }
        }
    }
}
