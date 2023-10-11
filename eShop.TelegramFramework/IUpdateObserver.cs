using Telegram.Bot.Types;

namespace eShop.TelegramFramework
{
    internal interface IUpdateObserver
    {
        event EventHandler<Update>? UpdateReceived;
    }
}
