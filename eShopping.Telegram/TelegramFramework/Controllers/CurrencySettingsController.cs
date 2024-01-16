using eShopping.Messaging;
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
