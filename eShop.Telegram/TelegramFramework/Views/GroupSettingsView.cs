using eShop.Telegram.Entities;
using eShop.Telegram.Models;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Builders;
using eShop.TelegramFramework.UI;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eShop.Telegram.TelegramFramework.Views
{
    public class GroupSettingsView : ITelegramView
    {
        private readonly long _chatId;
        private readonly int _messageId;
        private readonly TelegramChat _telegramChat;

        public GroupSettingsView(long chatId, int messageId, TelegramChat telegramChat)
        {
            _chatId = chatId;
            _messageId = messageId;
            _telegramChat = telegramChat;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            var text = $"Налаштування {(_telegramChat.Type == ChatType.Channel ? "каналу" : "групи")} {_telegramChat.Title}";

            var telegramChatSettings = _telegramChat.Settings;

            var elements = new IInlineKeyboardElement[]
            {
                new InlineKeyboardToggle("Увімкнути", "Ввимкнути", telegramChatSettings.IsEnabled, TelegramAction.SetGroupEnabled, _telegramChat.Id.ToString()),
            };
            var grid = new InlineKeyboardGrid(elements);
            var page = new InlineKeyboardPage(grid, TelegramAction.SetUpGroup, _telegramChat.Id.ToString())
            {
                Navigation = new InlineKeyboardNavigation(new InlineKeyboardAction("Назад", TelegramAction.MyGroups)),
            };

            var replyMarkup = markupBuilder.Build(page);
            await botClient.EditMessageTextAsync(new ChatId(_chatId), _messageId, text, replyMarkup: replyMarkup);
        }
    }
}
