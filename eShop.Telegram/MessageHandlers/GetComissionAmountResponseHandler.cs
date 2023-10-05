using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using Telegram.Bot;

namespace eShop.Telegram.MessageHandlers
{
    public class GetComissionAmountResponseHandler : IMessageHandler<GetComissionAmountResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramBotClient _botClient;
        private readonly IBotContextConverter _botContextConverter;

        public GetComissionAmountResponseHandler(ITelegramService telegramService, ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            _telegramService = telegramService;
            _botClient = botClient;
            _botContextConverter = botContextConverter;
        }

        public async Task HandleMessageAsync(GetComissionAmountResponse message)
        {
            var user = await _telegramService.GetUserByAccountIdAsync(message.AccountId);
            if (user != null)
            {
                var telegramView = new SetComissionAmountView(user.ExternalId, message.Amount);
                await telegramView.ProcessAsync(_botClient, _botContextConverter);
            }
        }
    }
}
