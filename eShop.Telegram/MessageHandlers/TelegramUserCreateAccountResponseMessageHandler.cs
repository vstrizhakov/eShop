using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.Telegram.MessageHandlers
{
    public class TelegramUserCreateAccountResponseMessageHandler : IMessageHandler<TelegramUserCreateAccountResponseMessage>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramBotClient _botClient;
        private readonly ITelegramViewRunner _telegramViewRunner;

        public TelegramUserCreateAccountResponseMessageHandler(
            ITelegramService telegramService,
            ITelegramBotClient botClient,
            ITelegramViewRunner telegramViewRunner)
        {
            _telegramService = telegramService;
            _botClient = botClient;
            _telegramViewRunner = telegramViewRunner;
        }

        public async Task HandleMessageAsync(TelegramUserCreateAccountResponseMessage message)
        {
            var user = await _telegramService.GetUserByTelegramUserIdAsync(message.TelegramUserId);
            if (user != null)
            {
                await _telegramService.SetAccountIdAsync(user, message.AccountId);

                var chatId = user.ExternalId;

                {
                    var replyMarkup = new ReplyKeyboardRemove();
                    var replyText = $"Вас успішно зареєстровано\n\n{message.ProviderEmail} встановлений як Ваш постачальник анонсів.";

                    await _botClient.SendTextMessageAsync(new ChatId(chatId), replyText, replyMarkup: replyMarkup);
                }

                var telegramView = new WelcomeView(chatId, null);
                await _telegramViewRunner.RunAsync(telegramView);
            }
        }
    }
}
