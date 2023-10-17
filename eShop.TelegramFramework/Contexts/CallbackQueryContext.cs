using Telegram.Bot.Types;

namespace eShop.TelegramFramework.Contexts
{
    public class CallbackQueryContext
    {
        public string Id { get; set; }
        public long FromId { get; }
        public long ChatId { get; }
        public int MessageId { get; }

        public CallbackQueryContext(Update update)
        {
            var callbackQuery = update.CallbackQuery!;
            Id = callbackQuery.Id;
            FromId = callbackQuery.From.Id;
            ChatId = callbackQuery.Message!.Chat.Id;
            MessageId = callbackQuery.Message!.MessageId;
        }
    }
}
