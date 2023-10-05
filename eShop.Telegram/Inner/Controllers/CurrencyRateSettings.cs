using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Inner.Attributes;
using eShop.Telegram.Inner.Contexts;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Models;
using eShop.Telegram.Services;

namespace eShop.Telegram.Inner.Controllers
{
    [TelegramController]
    public class CurrencyRateSettings : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;
        private readonly IProducer _producer;

        public CurrencyRateSettings(ITelegramService telegramService, IProducer producer)
        {
            _telegramService = telegramService;
            _producer = producer;
        }

        [CallbackQuery(TelegramAction.CurrencyRatesSettings)]
        public async Task<ITelegramView?> GetCurrencyRates(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetCurrencyRatesRequest(user.AccountId.Value);
                _producer.Publish(request);

            }

            return null;
        }

        [CallbackQuery(TelegramAction.SetCurrencyRate)]
        public async Task<ITelegramView?> GetCurrencyRate(CallbackQueryContext context, Guid currencyId)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetCurrencyRateRequest(user.AccountId.Value, currencyId);
                _producer.Publish(request);
            }

            return null;
        }

        [TextMessage(Action = TelegramAction.SetCurrencyRate)]
        public async Task<ITelegramView?> SetCurrencyRate(TextMessageContext context, Guid currencyId)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                if (double.TryParse(context.Text.Replace(',', '.'), out var rate))
                {
                    await _telegramService.SetActiveContextAsync(user, null);

                    var request = new SetCurrencyRateRequest(user.AccountId.Value, currencyId, rate);
                    _producer.Publish(request);
                }
                else
                {
                    // TODO: handle
                }
            }

            return null;
        }
    }
}
