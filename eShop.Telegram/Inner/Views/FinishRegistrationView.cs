using eShop.Bots.Common;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace eShop.Telegram.Inner.Views
{
    public class FinishRegistrationView : ITelegramView
    {
        private readonly long _chatId;

        public FinishRegistrationView(long chatId)
        {
            _chatId = chatId;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            var replyText = "Для завершення реєстрації надішліть свій номер телефону, будь ласка";
            var replyMarkup = new ReplyKeyboardMarkup(new KeyboardButton("Відправити номер телефону")
            {
                RequestContact = true,
            });

            await botClient.SendTextMessageAsync(new ChatId(_chatId), replyText, replyMarkup: replyMarkup);
        }
    }
}
