using eShop.Messaging;
using eShop.Messaging.Models.Telegram;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;

namespace eShop.Telegram.Handlers
{
    public class RegisterTelegramUserResponseHandler : IMessageHandler<RegisterTelegramUserResponse>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramViewRunner _viewRunner;

        public RegisterTelegramUserResponseHandler(ITelegramService telegramService, ITelegramViewRunner viewRunner)
        {
            _telegramService = telegramService;
            _viewRunner = viewRunner;
        }

        public async Task HandleMessageAsync(RegisterTelegramUserResponse response)
        {
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
                        new SuccessfullyRegisteredView(chatId, response.ProviderEmail),
                        new WelcomeView(chatId, null)
                    );
                }
            }
        }
    }
}
