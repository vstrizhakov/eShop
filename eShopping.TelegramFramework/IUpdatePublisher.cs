using Telegram.Bot.Types;

namespace eShopping.TelegramFramework
{
    public interface IUpdatePublisher
    {
        void Publish(Update update);
    }
}
