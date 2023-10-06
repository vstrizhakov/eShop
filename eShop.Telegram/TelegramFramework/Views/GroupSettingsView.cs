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
        private readonly TelegramChat _telegramChat;

        public GroupSettingsView(long chatId, TelegramChat telegramChat)
        {
            _chatId = chatId;
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
            var control = new InlineKeyboardList(elements)
            {
                Navigation = new InlineKeyboardAction("Назад", TelegramAction.MyGroups),
            };

            var replyMarkup = markupBuilder.Build(control);
            await botClient.SendTextMessageAsync(new ChatId(_chatId), text, replyMarkup: replyMarkup);
        }
    }
}
