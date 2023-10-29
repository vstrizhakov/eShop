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
    public class SettingsController
    {
        private readonly ITelegramService _telegramService;
        private readonly IRequestClient _requestClient;

        public SettingsController(ITelegramService telegramService, IRequestClient requestClient)
        {
            _telegramService = telegramService;
            _requestClient = requestClient;
        }

        [CallbackQuery(TelegramAction.Settings)]
        public async Task<ITelegramView?> ProcessAsync(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetDistributionSettingsRequest(user.AccountId.Value);
                var response = await _requestClient.SendAsync(request);

                return new SettingsView(context.ChatId, context.MessageId, response.DistributionSettings);
            }

            return null;
        }

        [CallbackQuery(TelegramAction.SetShowSales)]
        public async Task<ITelegramView?> SetShowSales(CallbackQueryContext context, bool showSales)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new SetShowSalesRequest(user.AccountId.Value, showSales);
                var response = await _requestClient.SendAsync(request);

                return new SettingsView(context.ChatId, context.MessageId, response.DistributionSettings);
            }

            return null;
        }
    }
}
