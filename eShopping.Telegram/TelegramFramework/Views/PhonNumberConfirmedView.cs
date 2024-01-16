using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Builders;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShopping.Telegram.TelegramFramework.Views
{
    public class PhonNumberConfirmedView : ITelegramView
    {
        private readonly long _chatId;

        public PhonNumberConfirmedView(long chatId)
        {
            _chatId = chatId;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var replyMarkup = new ReplyKeyboardRemove();
            var replyText = "Дякуємо! Ваш номер телефону успішно підтверждено.\nПоверніться на сторінку реєстрації для продовження роботи із сайтом.";

            await botClient.SendTextMessageAsync(new ChatId(_chatId), replyText, replyMarkup: replyMarkup);
        }
    }
}
