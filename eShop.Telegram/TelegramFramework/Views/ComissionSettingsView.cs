using eShop.Telegram.Models;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Builders;
using eShop.TelegramFramework.UI;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShop.Telegram.TelegramFramework.Views
{
    public class ComissionSettingsView : ITelegramView
    {
        private readonly long _chatId;
        private readonly bool _show;
        private readonly decimal _amount;

        public ComissionSettingsView(long chatId, bool show, decimal amount)
        {
            _chatId = chatId;
            _show = show;
            _amount = amount;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var text = "Налаштування комісій";

            var control = new InlineKeyboardList(new IInlineKeyboardElement[]
            {
                new InlineKeyboardToggle("Показувати комісію", "Не показувати комісію", _show, TelegramAction.SetComissionShow),
                new InlineKeyboardAction($"Розмір комісії ({_amount}%)", TelegramAction.SetComissionAmount),
            })
            {
                Navigation = new InlineKeyboardAction("Назад", TelegramAction.Settings),
            };

            var replyMarkup = markupBuilder.Build(control);

            await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
        }
    }
}
