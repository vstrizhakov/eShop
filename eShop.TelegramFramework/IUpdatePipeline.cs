using Telegram.Bot.Types;

namespace eShop.TelegramFramework
{
    internal interface IUpdatePipeline
    {
        Task HandleUpdateAsync(Update update);
    }
}
