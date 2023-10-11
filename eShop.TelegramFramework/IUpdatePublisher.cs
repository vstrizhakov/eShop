using Telegram.Bot.Types;

namespace eShop.TelegramFramework
{
    public interface IUpdatePublisher
    {
        void Publish(Update update);
    }
}
