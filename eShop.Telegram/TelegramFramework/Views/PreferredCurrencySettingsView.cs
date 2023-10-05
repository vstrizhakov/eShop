using eShop.Bots.Common;
using eShop.Messaging.Models;
using eShop.Telegram.Models;
using eShop.TelegramFramework;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.Telegram.TelegramFramework.Views
{
    public class PreferredCurrencySettingsView : ITelegramView
    {
        private readonly long _chatId;
        private readonly IEnumerable<Currency> _currencies;

        public PreferredCurrencySettingsView(long chatId, IEnumerable<Currency> currencies)
        {
            _chatId = chatId;
            _currencies = currencies;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            var text = "Оберіть одну з наступних валют як основу. Вона буде відображатися в ваших анонсах.";

            var buttons = new List<IEnumerable<InlineKeyboardButton>>();
            var maxButtons = 10;
            var maxCurrencies = maxButtons - 1;
            foreach (var currency in _currencies.Take(maxCurrencies))
            {
                var button = new InlineKeyboardButton(currency.Name)
                {
                    CallbackData = botContextConverter.Serialize(TelegramAction.SetPreferredCurrency, currency.Id.ToString()),
                };
                buttons.Add(new[] { button });
            }

            var backButton = new InlineKeyboardButton("Повернутися назад")
            {
                CallbackData = botContextConverter.Serialize(TelegramAction.CurrencySettings),
            };
            buttons.Add(new[] { backButton });

            var replyMarkup = new InlineKeyboardMarkup(buttons);
            await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
        }
    }
}
