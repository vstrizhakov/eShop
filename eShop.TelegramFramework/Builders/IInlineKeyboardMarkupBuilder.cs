using eShop.TelegramFramework.UI;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.TelegramFramework.Builders
{
    public interface IInlineKeyboardMarkupBuilder
    {
        InlineKeyboardMarkup Build<T>(T control) where T : IInlineKeyboardContainer;
    }

    public interface IInlineKeyboardMarkupBuilder<T> where T : IInlineKeyboardContainer
    {
        InlineKeyboardMarkup Build(T control);
    }
}
