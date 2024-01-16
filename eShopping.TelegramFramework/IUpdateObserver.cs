using Telegram.Bot.Types;

namespace eShopping.TelegramFramework
{
    internal interface IUpdateObserver
    {
        event EventHandler<Update>? UpdateReceived;
    }
}
