using Telegram.Bot.Types;

namespace eShopping.TelegramFramework
{
    internal class UpdateBridge : IUpdatePublisher, IUpdateObserver
    {
        public event EventHandler<Update>? UpdateReceived;

        public void Publish(Update update)
        {
            UpdateReceived?.Invoke(this, update);
        }
    }
}
