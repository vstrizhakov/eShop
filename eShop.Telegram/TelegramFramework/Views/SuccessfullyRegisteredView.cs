using eShop.Messaging.Contracts.Distribution;
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
        private readonly Announcer? _announcer;

        public SuccessfullyRegisteredView(long chatId, Announcer? announcer)
        {
            _chatId = chatId;
            _announcer = announcer;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var replyMarkup = new ReplyKeyboardRemove();
            var replyText = $"Вас успішно зареєстровано!";
            if (_announcer != null)
            {
                replyText += $"\n\n{_announcer.FirstName}";
                var lastName = _announcer.LastName;
                if (lastName != null)
                {
                    replyText += $" {lastName}";
                }
                replyText += " встановлений як Ваш постачальник анонсів.";
            }

            await botClient.SendTextMessageAsync(new ChatId(_chatId), replyText, replyMarkup: replyMarkup);
        }
    }
}
