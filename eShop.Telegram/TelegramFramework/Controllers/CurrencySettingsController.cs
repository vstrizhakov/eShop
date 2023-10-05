using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Models;
using eShop.Telegram.Services;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Attributes;
using eShop.TelegramFramework.Contexts;

namespace eShop.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class CurrencySettingsController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;
        private readonly IProducer _producer;

        public CurrencySettingsController(ITelegramService telegramService, IProducer producer)
        {
            _telegramService = telegramService;
            _producer = producer;
        }

        [CallbackQuery(TelegramAction.CurrencySettings)]
        public async Task<ITelegramView?> ProcessAsync(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetPreferredCurrencyRequest(user.AccountId.Value);
                _producer.Publish(request);
            }

            return null;
        }
    }
}
