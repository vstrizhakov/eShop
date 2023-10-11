using Telegram.Bot.Types;

namespace eShop.TelegramFramework
{
    internal class UpdateBridge : IUpdatePublisher, IUpdateObserver
    {
        public event EventHandler<Update>? UpdateArrived;

        public void Publish(Update update)
        {
            UpdateArrived?.Invoke(this, update);
        }
    }
}
