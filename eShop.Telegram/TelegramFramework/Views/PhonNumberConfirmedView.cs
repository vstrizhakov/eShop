using eShop.TelegramFramework;
using eShop.TelegramFramework.Builders;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShop.Telegram.TelegramFramework.Views
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
            var text = "Дякуємо! Ваш номер телефону успішно підтверждено.\nПоверніться на сторінку реєстрації для продовження роботи із сайтом.";

            await botClient.SendTextMessageAsync(new ChatId(_chatId), text);
        }
    }
}
