using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Models;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Attributes;
using eShop.TelegramFramework.Contexts;

namespace eShop.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class ComissionSettingsController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;
        private readonly IRequestClient _requestClient;
        private readonly IBotContextConverter _botContextConverter;

        public ComissionSettingsController(ITelegramService telegramService, IRequestClient requestClient, IBotContextConverter botContextConverter)
        {
            _telegramService = telegramService;
            _requestClient = requestClient;
            _botContextConverter = botContextConverter;
        }

        [CallbackQuery(TelegramAction.ComissionSettings)]
        public async Task<ITelegramView?> GetComissionSettings(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetComissionSettingsRequest(user.AccountId.Value);
                var response = await _requestClient.SendAsync(request);

                var view = new ComissionSettingsView(user.ExternalId, context.MessageId, response.Amount);
                return view;
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
                var response = await _requestClient.SendAsync(request);

                var view = new SetComissionAmountView(user.ExternalId, response.Amount);
                return view;
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
                    var response = await _requestClient.SendAsync(request);

                    await _telegramService.SetActiveContextAsync(user, null);

                    var view = new ComissionSettingsView(user.ExternalId, null, response.Amount);
                    return view;
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
