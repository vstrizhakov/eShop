using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Services;
using Telegram.Bot;

namespace eShop.Telegram.MessageHandlers
{
    public class SetPreferredCurrencyResponseHandler : IMessageHandler<SetPreferredCurrencyResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramBotClient _botClient;
        private readonly IBotContextConverter _botContextConverter;

        public SetPreferredCurrencyResponseHandler(ITelegramService telegramService, ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            _telegramService = telegramService;
            _botClient = botClient;
            _botContextConverter = botContextConverter;
        }

        public async Task HandleMessageAsync(SetPreferredCurrencyResponse message)
        {
            var user = await _telegramService.GetUserByAccountIdAsync(message.AccountId);
            if (user != null)
            {
                var telegramView = new CurrencySettingsView(user.ExternalId, message.PreferredCurrency);
                await telegramView.ProcessAsync(_botClient, _botContextConverter);
            }
        }
    }
}
