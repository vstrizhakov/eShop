using eShop.Bots.Common;
using Telegram.Bot;

namespace eShop.TelegramFramework
{
    public interface ITelegramView
    {
        Task ProcessAsync(ITelegramBotClient botClient, IBotContextConverter botContextConverter);
    }
}
