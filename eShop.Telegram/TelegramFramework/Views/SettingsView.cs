using eShop.Telegram.Models;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Builders;
using eShop.TelegramFramework.UI;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShop.Telegram.TelegramFramework.Views
{
    public class SettingsView : ITelegramView
    {
        private readonly long _chatId;

        public SettingsView(long chatId)
        {
            _chatId = chatId;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var text = "Налаштування";

            var elements = new IInlineKeyboardElement[]
            {
                new InlineKeyboardAction("Мої валюти", TelegramAction.CurrencySettings),
                new InlineKeyboardAction("Моя комісія", TelegramAction.ComissionSettings),
            };
            var control = new InlineKeyboardList(elements)
            {
                Navigation = new InlineKeyboardAction("Назад", TelegramAction.Home),
            };

            var replyMarkup = markupBuilder.Build(control);
            await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
        }
    }
}
