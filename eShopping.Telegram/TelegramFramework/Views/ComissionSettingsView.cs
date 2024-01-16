using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Builders;
using eShopping.TelegramFramework.UI;
using eShopping.Telegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShopping.Telegram.TelegramFramework.Views
{
    public class ComissionSettingsView : ITelegramView
    {
        private readonly long _chatId;
        private readonly int? _messageId;
        private readonly decimal _amount;

        public ComissionSettingsView(long chatId, int? messageId, decimal amount)
        {
            _chatId = chatId;
            _messageId = messageId;
            _amount = amount;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var text = "Налаштування комісій";

            var grid = new InlineKeyboardGrid(new IInlineKeyboardElement[]
            {
                new InlineKeyboardAction($"Розмір комісії ({_amount}%)", TelegramAction.SetComissionAmount),
            });
            var page = new InlineKeyboardPage(grid, TelegramAction.ComissionSettings)
            {
                Navigation = new InlineKeyboardNavigation(new InlineKeyboardAction("Назад", TelegramAction.Settings)),
            };
            var replyMarkup = markupBuilder.Build(page);

            if (!_messageId.HasValue)
            {
                await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
            }
            else
            {
                await botClient.EditMessageTextAsync(new ChatId(_chatId), _messageId.Value, text, replyMarkup: replyMarkup);
            }
        }
    }
}
