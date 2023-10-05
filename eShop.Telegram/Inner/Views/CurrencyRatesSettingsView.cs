using eShop.Bots.Common;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Distribution;
using eShop.Telegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.Telegram.Inner.Views
{
    public class CurrencyRatesSettingsView : ITelegramView
    {
        private readonly long _chatId;
        private readonly Currency _preferredCurrency;
        private readonly IEnumerable<CurrencyRate> _currencyRates;

        public CurrencyRatesSettingsView(long chatId, Currency preferredCurrency, IEnumerable<CurrencyRate> currencyRates)
        {
            _chatId = chatId;
            _preferredCurrency = preferredCurrency;
            _currencyRates = currencyRates;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            var text = $"Поточний курс валют відносно {_preferredCurrency.Name}\n\nНатисніть на валюту зі списку нижче, щоб змінити її курс.";
            var buttons = new List<IEnumerable<InlineKeyboardButton>>();
            foreach (var currencyRate in _currencyRates)
            {
                var currency = currencyRate.Currency;

                var buttonText = $"{currency.Name}: {currencyRate.Rate}";
                var button = new InlineKeyboardButton(buttonText)
                {
                    CallbackData = botContextConverter.Serialize(TelegramAction.SetCurrencyRate, currency.Id.ToString()),
                };

                buttons.Add(new[] { button });
            }

            var backButton = new InlineKeyboardButton("Назад")
            {
                CallbackData = botContextConverter.Serialize(TelegramAction.CurrencySettings),
            };

            buttons.Add(new[] { backButton });

            var replyMarkup = new InlineKeyboardMarkup(buttons);
            await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
        }
    }
}
