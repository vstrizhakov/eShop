using eShopping.Messaging.Contracts.Distribution;
using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Builders;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShopping.Telegram.TelegramFramework.Views
{
    public class SubscribedToAnnouncerView : ITelegramView
    {
        private readonly long _chatId;
        private readonly Announcer _announcer;

        public SubscribedToAnnouncerView(long chatId, Announcer announcer)
        {
            _chatId = chatId;
            _announcer = announcer;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var announcerName = _announcer.FirstName;
            var announcerLastName = _announcer.LastName;
            if (announcerLastName != null)
            {
                announcerName += $" {announcerLastName}";
            }

            await botClient.SendTextMessageAsync(new ChatId(_chatId), $"{announcerName} успішно встановлений як ваш постачальник анонсів.");
        }
    }
}
