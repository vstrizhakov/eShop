using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Inner.Contexts;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Models;
using eShop.Telegram.Services;

namespace eShop.Telegram.Inner.Controllers
{
    [TelegramController(TelegramAction.SetCurrencyRate, Context = TelegramContext.TextMessage)]
    public class SetCurrencyRateTextController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;
        private readonly IProducer _producer;

        public SetCurrencyRateTextController(ITelegramService telegramService, IProducer producer)
        {
            _telegramService = telegramService;
            _producer = producer;
        }

        public async Task<ITelegramView?> ProcessAsync(TextMessageContext context, Guid currencyId)
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
