using eShop.Bots.Common;
using eShop.Telegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.Telegram.Inner.Views
{
    public class SettingsView : ITelegramView
    {
        private readonly long _chatId;

        public SettingsView(long chatId)
        {
            _chatId = chatId;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            var text = "Налаштування";
            var replyMarkup = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    new InlineKeyboardButton("Мої валюти")
                    {
                        CallbackData = botContextConverter.Serialize(TelegramAction.CurrencySettings),
                    },
                },
            });
            await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
        }
    }
}
