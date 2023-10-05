using Telegram.Bot.Types;

namespace eShop.TelegramFramework.Contexts
{
    public class CallbackQueryContext
    {
        public long FromId { get; }
        public long ChatId { get; }

        public CallbackQueryContext(Update update)
        {
            var callbackQuery = update.CallbackQuery!;
            FromId = callbackQuery.From.Id;
            ChatId = callbackQuery.Message!.Chat.Id;
        }
    }
}
