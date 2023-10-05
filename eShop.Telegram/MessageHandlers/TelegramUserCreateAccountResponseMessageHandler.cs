using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.Telegram.MessageHandlers
{
    public class TelegramUserCreateAccountResponseMessageHandler : IMessageHandler<TelegramUserCreateAccountResponseMessage>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramBotClient _botClient;
        private readonly IBotContextConverter _botContextConverter;

        public TelegramUserCreateAccountResponseMessageHandler(
            ITelegramService telegramService,
            ITelegramBotClient botClient,
            IBotContextConverter botContextConverter)
        {
            _telegramService = telegramService;
            _botClient = botClient;
            _botContextConverter = botContextConverter;
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

                var telegramView = new WelcomeView(chatId);
                await telegramView.ProcessAsync(_botClient, _botContextConverter);
            }
        }
    }
}
