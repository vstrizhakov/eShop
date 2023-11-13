using eShop.Messaging;
using eShop.Messaging.Models.Distribution.ResetPassword;
using eShop.Telegram.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.Telegram.Handlers
{
    public class SendResetPasswordToTelegramMessageHandler : IMessageHandler<SendResetPasswordToTelegramMessage>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramBotClient _botClient;

        public SendResetPasswordToTelegramMessageHandler(ITelegramService telegramService, ITelegramBotClient botClient)
        {
            _telegramService = telegramService;
            _botClient = botClient;
        }

        public async Task HandleMessageAsync(SendResetPasswordToTelegramMessage message)
        {
            var user = await _telegramService.GetUserByTelegramUserIdAsync(message.TargetId);
            if (user != null)
            {
                var text = "Для відновлення доступу до Вашого акаунту перейдіть за посиланням, натиснувши кнопку нижче.";
                var replyMarkup = new InlineKeyboardMarkup(new InlineKeyboardButton("Відновити доступ")
                {
                    Url = message.ResetPasswordLink,
                });
                await _botClient.SendTextMessageAsync(new ChatId(user.ExternalId), text, replyMarkup: replyMarkup);
            }
        }
    }
}
