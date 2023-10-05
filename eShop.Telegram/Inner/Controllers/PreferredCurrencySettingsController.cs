using eShop.Messaging;
using eShop.Messaging.Models.Catalog;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Inner.Attributes;
using eShop.Telegram.Inner.Contexts;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Models;
using eShop.Telegram.Services;

namespace eShop.Telegram.Inner.Controllers
{
    [TelegramController]
    public class PreferredCurrencySettingsController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;
        private readonly IProducer _producer;

        public PreferredCurrencySettingsController(ITelegramService telegramService, IProducer producer)
        {
            _telegramService = telegramService;
            _producer = producer;
        }

        [CallbackQuery(TelegramAction.PreferredCurrencySettings)]
        public async Task<ITelegramView?> GetPreferredCurrency(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetCurrenciesRequest(user.AccountId.Value);
                _producer.Publish(request);
            }

            return null;
        }

        [CallbackQuery(TelegramAction.SetPreferredCurrency)]
        public async Task<ITelegramView?> SetPreferredCurrency(CallbackQueryContext context, Guid currencyId)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new SetPreferredCurrencyRequest(user.AccountId.Value, currencyId);
                _producer.Publish(request);
            }

            return null;
        }
    }
}
