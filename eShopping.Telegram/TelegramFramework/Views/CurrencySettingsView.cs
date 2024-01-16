using eShopping.Messaging.Contracts;
using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Builders;
using eShopping.TelegramFramework.UI;
using eShopping.Telegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShopping.Telegram.TelegramFramework.Views
{
    public class CurrencySettingsView : ITelegramView
    {
        private readonly long _chatId;
        private readonly int _messageId;
        private readonly Currency? _preferredCurrency;

        public CurrencySettingsView(long chatId, int messageId, Currency? preferredCurrency)
        {
            _chatId = chatId;
            _messageId = messageId;
            _preferredCurrency = preferredCurrency;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var text = "Мої валюти";

            var elements = new List<IInlineKeyboardElement>();

            {
                var caption = "Основна валюта";
                if (_preferredCurrency != null)
                {
                    caption += $" ({_preferredCurrency.Name})";
                }

                var element = new InlineKeyboardAction(caption, TelegramAction.PreferredCurrencySettings);
                elements.Add(element);
            }

            if (_preferredCurrency != null)
            {
                var caption = "Курси валют";
                var element = new InlineKeyboardAction(caption, TelegramAction.CurrencyRatesSettings);
                elements.Add(element);
            }

            var grid = new InlineKeyboardGrid(elements);
            var page = new InlineKeyboardPage(grid, TelegramAction.CurrencySettings)
            {
                Navigation = new InlineKeyboardNavigation(new InlineKeyboardAction("Назад", TelegramAction.Settings)),
            };

            var replyMarkup = markupBuilder.Build(page);
            await botClient.EditMessageTextAsync(new ChatId(_chatId), _messageId, text, replyMarkup: replyMarkup);
        }
    }
}
