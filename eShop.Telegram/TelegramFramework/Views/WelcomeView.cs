using eShop.Bots.Common;
using eShop.Telegram.Models;
using eShop.TelegramFramework;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.Telegram.TelegramFramework.Views
{
    public class WelcomeView : ITelegramView
    {
        private readonly long _chatId;

        public WelcomeView(long chatId)
        {
            _chatId = chatId;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            var text = "Доброго дня!";
            var replyMarkup = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    new InlineKeyboardButton("Мої групи")
                    {
                        CallbackData = botContextConverter.Serialize(TelegramAction.MyGroups),
                    },
                },
                new[]
                {
                    new InlineKeyboardButton("Налаштування")
                    {
                        CallbackData = botContextConverter.Serialize(TelegramAction.Settings),
                    },
                },
            });
            await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
        }
    }
}
