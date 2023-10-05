using eShop.Bots.Common;
using eShop.Messaging.Models;
using eShop.Telegram.Models;
using eShop.TelegramFramework;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.Telegram.TelegramFramework.Views
{
    public class CurrencySettingsView : ITelegramView
    {
        private readonly long _chatId;
        private readonly Currency? _preferredCurrency;

        public CurrencySettingsView(long chatId, Currency? preferredCurrency)
        {
            _chatId = chatId;
            _preferredCurrency = preferredCurrency;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            var text = "Мої валюти";

            var buttons = new List<IEnumerable<InlineKeyboardButton>>();

            {
                var buttonText = "Основна валюта";
                if (_preferredCurrency != null)
                {
                    buttonText += $" ({_preferredCurrency.Name})";
                }

                var button = new InlineKeyboardButton(buttonText)
                {
                    CallbackData = botContextConverter.Serialize(TelegramAction.PreferredCurrencySettings),
                };
                buttons.Add(new[] { button });
            }

            if (_preferredCurrency != null)
            {
                var buttonText = "Курси валют";
                var button = new InlineKeyboardButton(buttonText)
                {
                    CallbackData = botContextConverter.Serialize(TelegramAction.CurrencyRatesSettings),
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
