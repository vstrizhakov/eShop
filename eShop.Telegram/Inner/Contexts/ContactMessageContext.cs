using Telegram.Bot.Types;

namespace eShop.Telegram.Inner.Contexts
{
    public class ContactMessageContext
    {
        public Contact Contact { get; }
        public long FromId { get; }
        public long ChatId { get; }

        public ContactMessageContext(Update update)
        {
            var message = update.Message!;
            Contact = message.Contact!;
            FromId = message.From!.Id;
            ChatId = message.Chat.Id;
        }
    }
}
