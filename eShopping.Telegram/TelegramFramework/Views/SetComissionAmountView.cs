using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Builders;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShopping.Telegram.TelegramFramework.Views
{
    public class SetComissionAmountView : ITelegramView
    {
        private readonly long _chatId;
        private readonly decimal _amount;

        public SetComissionAmountView(long chatId, decimal amount)
        {
            _chatId = chatId;
            _amount = amount;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var text = $"Ваша поточна комісія: {_amount}%\n\nНадішліть новий розмір комісії.";
            var replyMarkup = new ForceReplyMarkup();
            await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
        }
    }
}
