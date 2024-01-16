using eShopping.Messaging.Contracts.Distribution;
using eShopping.Telegram.TelegramFramework.Views;
using eShopping.TelegramFramework;
using eShopping.Telegram.Services;
using MassTransit;

namespace eShopping.Telegram.Consumers
{
    public class SubscribeToAnnouncerResponseConsumer : IConsumer<SubscribeToAnnouncerResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramViewRunner _viewRunner;

        public SubscribeToAnnouncerResponseConsumer(ITelegramService telegramService, ITelegramViewRunner viewRunner)
        {
            _telegramService = telegramService;
            _viewRunner = viewRunner;
        }

        public async Task Consume(ConsumeContext<SubscribeToAnnouncerResponse> context)
        {
            var response = context.Message;
            var telegramUserId = response.TelegramUserId;
            if (telegramUserId.HasValue)
            {
                var user = await _telegramService.GetUserByTelegramUserIdAsync(telegramUserId.Value);
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
}
