using eShop.Messaging;
using eShop.Messaging.Contracts.Distribution;
using eShop.Telegram.Models;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Attributes;
using eShop.TelegramFramework.Contexts;
using MassTransit;

namespace eShop.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class CurrencySettingsController
    {
        private readonly ITelegramService _telegramService;
        private readonly IRequestClient<GetPreferredCurrencyRequest> _getPreferredCurrencyRequestClient;

        public CurrencySettingsController(ITelegramService telegramService, IRequestClient<GetPreferredCurrencyRequest> getPreferredCurrencyRequestClient)
        {
            _telegramService = telegramService;
            _getPreferredCurrencyRequestClient = getPreferredCurrencyRequestClient;
        }

        [CallbackQuery(TelegramAction.CurrencySettings)]
        public async Task<ITelegramView?> ProcessAsync(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetPreferredCurrencyRequest(user.AccountId.Value);
                var result = await _getPreferredCurrencyRequestClient.GetResponse<GetPreferredCurrencyResponse>(request);
                var response = result.Message;

                var view = new CurrencySettingsView(user.ExternalId, context.MessageId, response.PreferredCurrency);
                return view;
            }

            return null;
        }
    }
}
