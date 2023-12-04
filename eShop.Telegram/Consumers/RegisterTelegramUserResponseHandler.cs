using eShop.Messaging.Contracts.Telegram;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using MassTransit;

namespace eShop.Telegram.Consumers
{
    public class RegisterTelegramUserResponseHandler : IConsumer<RegisterTelegramUserResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramViewRunner _viewRunner;

        public RegisterTelegramUserResponseHandler(ITelegramService telegramService, ITelegramViewRunner viewRunner)
        {
            _telegramService = telegramService;
            _viewRunner = viewRunner;
        }

        public async Task Consume(ConsumeContext<RegisterTelegramUserResponse> context)
        {
            var response = context.Message;
            var user = await _telegramService.GetUserByTelegramUserIdAsync(response.TelegramUserId);
            if (user != null)
            {
                await _telegramService.SetAccountIdAsync(user, response.AccountId);

                var chatId = user.ExternalId;
                if (response.IsConfirmationRequested)
                {
                    await _viewRunner.RunAsync(
                        new PhonNumberConfirmedView(chatId)
                    );
                }
                else
                {
                    await _viewRunner.RunAsync(
                        new SuccessfullyRegisteredView(chatId, response.Announcer),
                        new WelcomeView(chatId, null)
                    );
                }
            }
        }
    }
}
