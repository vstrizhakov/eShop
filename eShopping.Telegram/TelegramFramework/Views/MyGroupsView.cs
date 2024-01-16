using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Builders;
using eShopping.TelegramFramework.UI;
using eShopping.Telegram.Entities;
using eShopping.Telegram.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace eShopping.Telegram.TelegramFramework.Views
{
    public class MyGroupsView : ITelegramView
    {
        private readonly long _chatId;
        private readonly int _messageId;
        private readonly string _callbackQueryId;
        private readonly IEnumerable<TelegramChat> _chats;

        public MyGroupsView(long chatId, int messageId, string callbackQueryId, IEnumerable<TelegramChat> chats)
        {
            _chatId = chatId;
            _messageId = messageId;
            _callbackQueryId = callbackQueryId;
            _chats = chats;
        }

        public async Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder)
        {
            if (_chats.Any())
            {
                var text = "Оберіть групу чи канал, до якої хотіли б налаштувати відправку анонсів:";

                var elements = new List<IInlineKeyboardElement>();
                foreach (var chat in _chats)
                {
                    var element = new InlineKeyboardAction(chat.Title!, TelegramAction.SetUpGroup, chat.Id.ToString());
                    elements.Add(element);
                }

                var grid = new InlineKeyboardGrid(elements);
                var page = new InlineKeyboardPage(grid, TelegramAction.MyGroups)
                {
                    Navigation = new InlineKeyboardNavigation(new InlineKeyboardAction("Назад", TelegramAction.Home)),
                };

                var replyMarkup = markupBuilder.Build(page);
                await botClient.EditMessageTextAsync(new ChatId(_chatId), _messageId, text, replyMarkup: replyMarkup);
            }
            else
            {
                var text = "Додайте бота до групи чи каналу, у який хочете налаштувати відправку анонсів, і натисніть кнопку Оновити нижче.";

                var elements = new IInlineKeyboardElement[]
                {
                    new InlineKeyboardAction("Оновити", TelegramAction.Refresh)
                };
                var grid = new InlineKeyboardGrid(elements);
                var page = new InlineKeyboardPage(grid, TelegramAction.MyGroups)
                {
                    Navigation = new InlineKeyboardNavigation(new InlineKeyboardAction("Назад", TelegramAction.Home)),
                };

                var replyMarkup = markupBuilder.Build(page);
                try
                {
                    await botClient.EditMessageTextAsync(new ChatId(_chatId), _messageId, text, replyMarkup: replyMarkup);
                }
                catch (ApiRequestException)
                {
                    await botClient.AnswerCallbackQueryAsync(_callbackQueryId);

                    // TODO: ignore but on another level
                }
            }
        }
    }
}
