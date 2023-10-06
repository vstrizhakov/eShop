using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using Telegram.Bot;

namespace eShop.Telegram.MessageHandlers
{
    public class SetComissionAmountResponseHandler : IMessageHandler<SetComissionAmountResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramViewRunner _telegramViewRunner;

        public SetComissionAmountResponseHandler(ITelegramService telegramService, ITelegramViewRunner telegramViewRunner)
        {
            _telegramService = telegramService;
            _telegramViewRunner = telegramViewRunner;
        }

        public async Task HandleMessageAsync(SetComissionAmountResponse message)
        {
            var user = await _telegramService.GetUserByAccountIdAsync(message.AccountId);
            if (user != null)
            {
                await _telegramService.SetActiveContextAsync(user, null);

                var telegramView = new ComissionSettingsView(user.ExternalId, message.Show, message.Amount);
                await _telegramViewRunner.RunAsync(telegramView);
            }
        }
    }
}
