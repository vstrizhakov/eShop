using eShop.Bots.Common;
using eShop.Telegram.Entities;
using eShop.Telegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.Telegram.Inner.Views
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

        public async Task ProcessAsync(ITelegramBotClient botClient, IBotContextConverter botContextConverter)
        {
            var replyText = $"Налаштування {(_telegramChat.Type == ChatType.Channel ? "каналу" : "групи")} {_telegramChat.Title}";
            var lines = new List<List<InlineKeyboardButton>>();

            var telegramChatSettings = _telegramChat.Settings;

            var firstLine = new List<InlineKeyboardButton>();
            if (telegramChatSettings.IsEnabled)
            {
                firstLine.Add(new InlineKeyboardButton("Ввимкнути")
                {
                    CallbackData = botContextConverter.Serialize(TelegramAction.SettingsDisable, _telegramChat.Id.ToString()),
                });
            }
            else
            {
                firstLine.Add(new InlineKeyboardButton("Увімкнути")
                {
                    CallbackData = botContextConverter.Serialize(TelegramAction.SettingsEnable, _telegramChat.Id.ToString()),
                });
            }

            lines.Add(firstLine);

            var replyMarkup = new InlineKeyboardMarkup(lines);
            await botClient.SendTextMessageAsync(new ChatId(_chatId), replyText, replyMarkup: replyMarkup);
        }
    }
}
