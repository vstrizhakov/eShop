using eShopping.Messaging.Contracts;
using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Builders;
using eShopping.TelegramFramework.UI;
using eShopping.Telegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShopping.Telegram.TelegramFramework.Views
{
    public class PreferredCurrencySettingsView : ITelegramView
    {
        private readonly long _chatId;
        private readonly int _messageId;
        private readonly IEnumerable<Currency> _currencies;

        public PreferredCurrencySettingsView(long chatId, int messageId, IEnumerable<Currency> currencies)
        {
            _chatId = chatId;
            _messageId = messageId;
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

            var grid = new InlineKeyboardGrid(elements);
            var page = new InlineKeyboardPage(grid, TelegramAction.PreferredCurrencySettings)
            {
                Navigation = new InlineKeyboardNavigation(new InlineKeyboardAction("Повернутися назад", TelegramAction.CurrencySettings)),
            };

            var replyMarkup = markupBuilder.Build(page);
            await botClient.EditMessageTextAsync(new ChatId(_chatId), _messageId, text, replyMarkup: replyMarkup);
        }
    }
}
