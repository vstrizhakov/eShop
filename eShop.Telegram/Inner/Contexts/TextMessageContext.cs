using Telegram.Bot.Types;

namespace eShop.Telegram.Inner.Contexts
{
    public class TextMessageContext
    {
        public long FromId { get; }
        public long ChatId { get; }
        public string Text { get; }

        public TextMessageContext(Update update)
        {
            var message = update.Message!;
            FromId = message.From!.Id;
            ChatId = message.Chat.Id;
            Text = message.Text!;
        }
    }
}
