using eShop.Bots.Common;
using Telegram.Bot;

namespace eShop.Telegram.Inner.Views
{
    public interface ITelegramView
    {
        Task ProcessAsync(ITelegramBotClient botClient, IBotContextConverter botContextConverter);
    }
}
