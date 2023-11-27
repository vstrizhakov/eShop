using eShop.Bots.Common;
using eShop.Messaging.Contracts;
using eShop.Messaging.Contracts.Distribution;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Builders;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

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

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var sourceCurrency = _currencyRate.Currency;
            var text = $"{sourceCurrency.Name} -> {_preferredCurrency.Name}\n\nПоточний курс: {_currencyRate.Rate}\n\nБудь ласка, надішліть новий курс для цієї пари.";

            var replyMarkup = new ForceReplyMarkup();
            await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
        }
    }
}
