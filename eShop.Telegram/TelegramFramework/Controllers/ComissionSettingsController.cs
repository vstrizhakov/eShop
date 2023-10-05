using eShop.Bots.Common;
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
    public class ComissionSettingsController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;
        private readonly IProducer _producer;
        private readonly IBotContextConverter _botContextConverter;

        public ComissionSettingsController(ITelegramService telegramService, IProducer producer, IBotContextConverter botContextConverter)
        {
            _telegramService = telegramService;
            _producer = producer;
            _botContextConverter = botContextConverter;
        }

        [CallbackQuery(TelegramAction.ComissionSettings)]
        public async Task<ITelegramView?> GetComissionSettings(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetComissionSettingsRequest(user.AccountId.Value);
                _producer.Publish(request);
            }

            return null;
        }

        [CallbackQuery(TelegramAction.SetComissionShow)]
        public async Task<ITelegramView?> SetComissionShow(CallbackQueryContext context, bool show)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new SetComissionShowRequest(user.AccountId.Value, show);
                _producer.Publish(request);
            }

            return null;
        }

        [CallbackQuery(TelegramAction.SetComissionAmount)]
        public async Task<ITelegramView?> SetComissionAmount(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var activeContext = _botContextConverter.Serialize(TelegramAction.SetComissionAmount);
                await _telegramService.SetActiveContextAsync(user, activeContext);

                var request = new GetComissionAmountRequest(user.AccountId.Value);
                _producer.Publish(request);
            }

            return null;
        }

        [TextMessage(Action = TelegramAction.SetComissionAmount)]
        public async Task<ITelegramView?> SetComissionAmount(TextMessageContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                if (decimal.TryParse(context.Text.Replace(',', '.'), out var amount))
                {
                    var request = new SetComissionAmountRequest(user.AccountId.Value, amount);
                    _producer.Publish(request);
                }
                else
                {
                    // TODO:
                }
            }

            return null;
        }
    }
}
