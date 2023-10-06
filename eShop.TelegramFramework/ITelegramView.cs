using eShop.TelegramFramework.Builders;
using Telegram.Bot;

namespace eShop.TelegramFramework
{
    public interface ITelegramView
    {
        Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder);
    }
}
