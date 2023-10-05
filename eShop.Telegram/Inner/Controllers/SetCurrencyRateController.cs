using eShop.Messaging.Models.Distribution;
using eShop.Messaging;
using eShop.Telegram.Inner.Contexts;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Models;
using eShop.Telegram.Services;
using eShop.Messaging.Models.Catalog;

namespace eShop.Telegram.Inner.Controllers
{
    [TelegramController(TelegramAction.SetCurrencyRate, Context = TelegramContext.CallbackQuery)]
    public class SetCurrencyRateController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;
        private readonly IProducer _producer;
        public SetCurrencyRateController(ITelegramService telegramService, IProducer producer)
        {
            _telegramService = telegramService;
            _producer = producer;
        }

        public async Task<ITelegramView?> ProcessAsync(CallbackQueryContext context, Guid currencyId)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetCurrencyRateRequest(user.AccountId.Value, currencyId);
                _producer.Publish(request);
            }

            return null;
        }
    }
}
