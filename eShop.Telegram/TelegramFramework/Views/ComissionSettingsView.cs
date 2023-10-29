using eShop.Telegram.Models;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Builders;
using eShop.TelegramFramework.UI;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShop.Telegram.TelegramFramework.Views
{
    public class ComissionSettingsView : ITelegramView
    {
        private readonly long _chatId;
        private readonly int? _messageId;
        private readonly double _amount;

        public ComissionSettingsView(long chatId, int? messageId, double amount)
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
