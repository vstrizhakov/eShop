using eShopping.TelegramFramework.UI;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShopping.TelegramFramework.Builders
{
    internal interface IInlineKeyboardButtonBuilder
    {
        InlineKeyboardButton Build<T>(T element) where T : IInlineKeyboardElement;
    }

    internal interface IInlineKeyboardButtonBuilder<T> where T : IInlineKeyboardElement
    {
        InlineKeyboardButton Build(T element);
    }
}
