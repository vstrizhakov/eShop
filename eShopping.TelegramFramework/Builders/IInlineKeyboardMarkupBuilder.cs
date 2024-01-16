using eShopping.TelegramFramework.UI;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShopping.TelegramFramework.Builders
{
    public interface IInlineKeyboardMarkupBuilder
    {
        InlineKeyboardMarkup Build<T>(T control) where T : IInlineKeyboardContainer;
    }

    public interface IInlineKeyboardMarkupBuilder<T> where T : IInlineKeyboardContainer
    {
        IEnumerable<IEnumerable<InlineKeyboardButton>> Build(T control);
    }
}
