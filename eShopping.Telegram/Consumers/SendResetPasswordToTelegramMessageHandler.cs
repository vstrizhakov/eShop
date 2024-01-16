using eShopping.Messaging.Contracts.Distribution.ResetPassword;
using eShopping.Telegram.Services;
using MassTransit;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShopping.Telegram.Consumers
{
    public class SendResetPasswordToTelegramMessageHandler : IConsumer<SendResetPasswordToTelegramMessage>
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramBotClient _botClient;

        public SendResetPasswordToTelegramMessageHandler(ITelegramService telegramService, ITelegramBotClient botClient)
        {
            _telegramService = telegramService;
            _botClient = botClient;
        }

        public async Task Consume(ConsumeContext<SendResetPasswordToTelegramMessage> context)
        {
            var command = context.Message;
            var user = await _telegramService.GetUserByTelegramUserIdAsync(command.TargetId);
            if (user != null)
            {
                var text = "Для відновлення доступу до Вашого акаунту перейдіть за посиланням, натиснувши кнопку нижче.";
                var replyMarkup = new InlineKeyboardMarkup(new InlineKeyboardButton("Відновити доступ")
                {
                    Url = command.ResetPasswordLink,
                });
                await _botClient.SendTextMessageAsync(new ChatId(user.ExternalId), text, replyMarkup: replyMarkup);
            }
        }
    }
}
