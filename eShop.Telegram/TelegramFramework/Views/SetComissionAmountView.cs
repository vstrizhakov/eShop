using eShop.Bots.Common;
using eShop.TelegramFramework;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShop.Telegram.TelegramFramework.Views
{
    public class SetComissionAmountView : ITelegramView
    {
        private long _chatId;
        private decimal _amount;

        public SetComissionAmountView(long chatId, decimal amount)
        {
            _chatId = chatId;
            _amount = amount;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            var text = $"Ваша поточна комісія: {_amount}%\n\nНадішліть новий розмір комісії.";
            await botClient.SendTextMessageAsync(new ChatId(_chatId), text);
        }
    }
}
