using eShop.Messaging.Models;
using eShop.Telegram.Models;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Builders;
using eShop.TelegramFramework.UI;
using Telegram.Bot;
using Telegram.Bot.Types;

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

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var text = "Оберіть одну з наступних валют як основу. Вона буде відображатися в ваших анонсах.";

            var elements = new List<IInlineKeyboardElement>();
            foreach (var currency in _currencies)
            {
                var element = new InlineKeyboardAction(currency.Name, TelegramAction.SetPreferredCurrency, currency.Id.ToString());
                elements.Add(element);
            }

            var control = new InlineKeyboardList(elements)
            {
                Navigation = new InlineKeyboardAction("Повернутися назад", TelegramAction.CurrencySettings),
            };

            var replyMarkup = markupBuilder.Build(control);
            await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
        }
    }
}
