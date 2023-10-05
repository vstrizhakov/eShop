using eShop.Bots.Common;
using eShop.Telegram.Models;
using eShop.TelegramFramework;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

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

        public async Task ProcessAsync(ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            var text = "Налаштування комісій";

            var buttons = new List<IEnumerable<InlineKeyboardButton>>();

            {
                var buttonText = _show ? "Не показувати комісію" : "Показувати комісію";
                var button = new InlineKeyboardButton(buttonText)
                {
                    CallbackData = botContextConverter.Serialize(TelegramAction.SetComissionShow, (!_show).ToString()),
                };

                buttons.Add(new[] { button });
            }

            {
                var buttonText = $"Розмір комісії ({_amount}%)";
                var button = new InlineKeyboardButton(buttonText)
                {
                    CallbackData = botContextConverter.Serialize(TelegramAction.SetComissionAmount),
                };

                buttons.Add(new[] { button });
            }

            {
                var buttonText = "Назад";
                var button = new InlineKeyboardButton(buttonText)
                {
                    CallbackData = botContextConverter.Serialize(TelegramAction.Settings),
                };

                buttons.Add(new[] { button });
            }

            var replyMarkup = new InlineKeyboardMarkup(buttons);

            await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
        }
    }
}
