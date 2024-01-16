using Telegram.Bot.Types;

namespace eShopping.TelegramFramework
{
    internal interface IUpdatePipeline
    {
        Task HandleUpdateAsync(Update update);
    }
}
