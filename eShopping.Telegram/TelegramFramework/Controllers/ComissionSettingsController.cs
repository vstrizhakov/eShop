using eShopping.Bots.Common;
using eShopping.Messaging.Contracts.Distribution;
using eShopping.Telegram.TelegramFramework.Views;
using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Attributes;
using eShopping.TelegramFramework.Contexts;
using eShopping.Telegram.Models;
using eShopping.Telegram.Services;
using MassTransit;

namespace eShopping.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class ComissionSettingsController
    {
        private readonly ITelegramService _telegramService;
        private readonly IRequestClient<GetComissionSettingsRequest> _getComissionSettingsRequestClient;
        private readonly IRequestClient<GetComissionAmountRequest> _getComissionAmountRequestClient;
        private readonly IRequestClient<SetComissionAmountRequest> _setComissionAmounRequestClient;
        private readonly IBotContextConverter _botContextConverter;

        public ComissionSettingsController(
            ITelegramService telegramService,
            IRequestClient<GetComissionSettingsRequest> getComissionSettingsRequestClient,
            IBotContextConverter botContextConverter,
            IRequestClient<GetComissionAmountRequest> getComissionAmountRequestClient,
            IRequestClient<SetComissionAmountRequest> setComissionAmounRequestClient)
        {
            _telegramService = telegramService;
            _getComissionSettingsRequestClient = getComissionSettingsRequestClient;
            _botContextConverter = botContextConverter;
            _getComissionAmountRequestClient = getComissionAmountRequestClient;
            _setComissionAmounRequestClient = setComissionAmounRequestClient;
        }

        [CallbackQuery(TelegramAction.ComissionSettings)]
        public async Task<ITelegramView?> GetComissionSettings(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetComissionSettingsRequest(user.AccountId.Value);
                var response = await _getComissionSettingsRequestClient.GetResponse<GetComissionSettingsResponse>(request);

                var view = new ComissionSettingsView(user.ExternalId, context.MessageId, response.Message.Amount);
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
                var response = await _getComissionAmountRequestClient.GetResponse<GetComissionAmountResponse>(request);

                var view = new SetComissionAmountView(user.ExternalId, response.Message.Amount);
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
                    var response = await _setComissionAmounRequestClient.GetResponse<SetComissionAmountResponse>(request);

                    await _telegramService.SetActiveContextAsync(user, null);

                    var view = new ComissionSettingsView(user.ExternalId, null, response.Message.Amount);
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
