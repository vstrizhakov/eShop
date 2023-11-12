using eShop.TelegramFramework;
using eShop.TelegramFramework.Builders;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.Telegram.TelegramFramework.Views
{
    public class SuccessfullyRegisteredView : ITelegramView
    {
        private readonly long _chatId;
        private readonly string _providerEmail;

        public SuccessfullyRegisteredView(long chatId, string providerEmail)
        {
            _chatId = chatId;
            _providerEmail = providerEmail;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var replyMarkup = new ReplyKeyboardRemove();
            var replyText = $"Вас успішно зареєстровано\n\n{_providerEmail} встановлений як Ваш постачальник анонсів.";

            await botClient.SendTextMessageAsync(new ChatId(_chatId), replyText, replyMarkup: replyMarkup);
        }
    }
}
