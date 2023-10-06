using eShop.TelegramFramework;
using eShop.TelegramFramework.Builders;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShop.Telegram.TelegramFramework.Views
{
    public class AlreadyRegisteredView : ITelegramView
    {
        private readonly long _chatId;

        public AlreadyRegisteredView(long chatId)
        {
            _chatId = chatId;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            await botClient.SendTextMessageAsync(new ChatId(_chatId), "Ви вже зареєстровані та маєтє встановленного постачальника анонсів");
        }
    }
}
