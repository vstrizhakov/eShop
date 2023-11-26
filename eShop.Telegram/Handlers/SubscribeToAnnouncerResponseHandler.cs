using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;

namespace eShop.Telegram.Handlers
{
    public class SubscribeToAnnouncerResponseHandler : IMessageHandler<SubscribeToAnnouncerResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramViewRunner _viewRunner;

        public SubscribeToAnnouncerResponseHandler(ITelegramService telegramService, ITelegramViewRunner viewRunner)
        {
            _telegramService = telegramService;
            _viewRunner = viewRunner;
        }

        public async Task HandleMessageAsync(SubscribeToAnnouncerResponse response)
        {
            var user = await _telegramService.GetUserByAccountIdAsync(response.AccountId);
            if (user != null)
            {
                if (response.Succeeded)
                {
                    var view = new SubscribedToAnnouncerView(user.ExternalId, response.Announcer);
                    await _viewRunner.RunAsync(view);
                }
            }
        }
    }
}
