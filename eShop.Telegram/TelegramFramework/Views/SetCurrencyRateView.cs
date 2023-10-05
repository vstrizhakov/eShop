using eShop.Bots.Common;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Distribution;
using eShop.TelegramFramework;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShop.Telegram.TelegramFramework.Views
{
    public class SetCurrencyRateView : ITelegramView
    {
        private readonly long _chatId;
        private readonly Currency _preferredCurrency;
        private readonly CurrencyRate _currencyRate;

        public SetCurrencyRateView(long chatId, Currency preferredCurrency, CurrencyRate currencyRate)
        {
            _chatId = chatId;
            _preferredCurrency = preferredCurrency;
            _currencyRate = currencyRate;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            var sourceCurrency = _currencyRate.Currency;
            var text = $"{sourceCurrency.Name} -> {_preferredCurrency.Name}\n\nПоточний курс: {_currencyRate.Rate}\n\nБудь ласка, надішліть новий курс для цієї пари.";

            await botClient.SendTextMessageAsync(new ChatId(_chatId), text);
        }
    }
}
