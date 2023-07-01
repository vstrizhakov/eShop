using eShop.Bots.Common;
using eShop.Messaging.Extensions;
using eShop.Messaging.Models;
using eShop.RabbitMq;
using eShop.Telegram.DbContexts;
using eShop.Telegram.Entities;
using eShop.Telegram.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.Telegram.Controllers
{
    [Route(TelegramBotConfiguration.WebhookRoute)]
    [ApiController]
    public class TelegramWebhookController : ControllerBase
    {
        private readonly TelegramDbContext _context;
        private readonly ITelegramBotClient _botClient;
        private readonly IRabbitMqProducer _producer;

        public TelegramWebhookController(
            TelegramDbContext context,
            ITelegramBotClient botClient,
            IRabbitMqProducer producer)
        {
            _context = context;
            _botClient = botClient;
            _producer = producer;
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] Update update,
            [FromServices] IBotContextConverter botContextConverter,
            CancellationToken cancellationToken)
        {
            Message? message = null;
            if (update.Type == UpdateType.Message)
            {
                message = update.Message;
            }
            else if (update.Type == UpdateType.ChannelPost)
            {
                message = update.ChannelPost;
            }

            if (message != null)
            {
                var chat = message.Chat;

                var chatId = chat.Id;
                var telegramChat = await _context.TelegramChats
                    .Include(e => e.Members)
                    .FirstOrDefaultAsync(e => e.ExternalId == chatId);
                if (telegramChat == null)
                {
                    telegramChat = new TelegramChat
                    {
                        ExternalId = chatId,
                    };

                    _context.TelegramChats.Add(telegramChat);

                    if (chat.Type == ChatType.Group || chat.Type == ChatType.Supergroup || chat.Type == ChatType.Channel)
                    {
                        var chatAdministrators = await _botClient.GetChatAdministratorsAsync(new ChatId(chatId));

                        foreach (var chatAdministrator in chatAdministrators)
                        {
                            await AddOrUpdateTelegramUserAsync(telegramChat, chatAdministrator.User, chatAdministrator.Status);
                        }
                    }
                }

                telegramChat.Type = chat.Type;
                telegramChat.Title = chat.Title;

                if (message.Type == MessageType.MigratedFromGroup)
                {
                    var baseTelegramChat = await _context.TelegramChats
                        .Include(e => e.Supergroup)
                        .FirstOrDefaultAsync(e => e.ExternalId == message.MigrateFromChatId);
                    if (baseTelegramChat != null)
                    {
                        baseTelegramChat.Supergroup = telegramChat;
                        foreach (var member in baseTelegramChat.Members)
                        {
                            telegramChat.Members.Add(member);
                        }
                    }
                }

                var from = message.From;
                if (from != null)
                {
                    await AddOrUpdateTelegramUserAsync(telegramChat, from);
                }

                await _context.SaveChangesAsync();

                if (chat.Type == ChatType.Private)
                {
                    if (message.Type == MessageType.Text)
                    {
                        var text = message.Text!;

                        var command = text;
                        var arguments = string.Empty;

                        var seperatorIndex = text.IndexOf(' ');
                        if (seperatorIndex != -1)
                        {
                            command = text.Substring(0, seperatorIndex);
                            arguments = text.Substring(seperatorIndex + 1, text.Length - seperatorIndex - 1);
                        }

                        switch (command)
                        {
                            case "/start":
                                //arguments = botContextConverter.Serialize(TelegramContext.Action.RegisterClient, Guid.NewGuid().ToString());
                                if (!string.IsNullOrEmpty(arguments))
                                {
                                    var context = botContextConverter.Deserialize(arguments);
                                    if (context.Length > 0)
                                    {
                                        var action = context[0];

                                        if (action == TelegramContext.Action.RegisterClient)
                                        {
                                            if (context.Length > 1)
                                            {
                                                if (Guid.TryParse(context[1], out var providerId))
                                                {
                                                    var telegramUser = (await _context.TelegramUsers
                                                        .FirstOrDefaultAsync(e => e.ExternalId == from!.Id))!;

                                                    if (telegramUser.AccountId == null)
                                                    {
                                                        var internalMessage = new TelegramUserCreateAccountRequestMessage
                                                        {
                                                            TelegramUserId = telegramUser.Id,
                                                            FirstName = telegramUser.FirstName,
                                                            LastName = telegramUser.LastName,
                                                            ProviderId = providerId,
                                                        };
                                                        _producer.Publish(internalMessage);
                                                    }
                                                    else
                                                    {
                                                        await _botClient.SendTextMessageAsync(new ChatId(chatId), "Ви вже зареєстровані та маєтє встановленного постачальника анонсів");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var telegramUser = (await _context.TelegramUsers
                                        .Include(e => e.Chats)
                                            .ThenInclude(e => e.Chat)
                                        .FirstOrDefaultAsync(e => e.ExternalId == from!.Id))!;
                                    if (telegramUser.AccountId != null)
                                    {
                                        var telegramUserChats = telegramUser.Chats
                                            .Where(e => e.Chat.Type == ChatType.Group || e.Chat.Type == ChatType.Channel || e.Chat.Type == ChatType.Supergroup)
                                            .Where(e => e.Chat.SupergroupId == null)
                                            .Where(e => e.Status == ChatMemberStatus.Creator || e.Status == ChatMemberStatus.Administrator);
                                        if (telegramUserChats.Any())
                                        {
                                            var replyText = "Оберіть групу чи канал, до якої хотіли б налаштувати відправку анонсів:";
                                            var replyMarkup = new InlineKeyboardMarkup(telegramUserChats.Select(e =>
                                            {
                                                var chat = e.Chat;
                                                return new List<InlineKeyboardButton>()
                                                {
                                                    new InlineKeyboardButton(chat.Title!)
                                                    {
                                                        CallbackData = botContextConverter.Serialize(TelegramContext.Action.SetUpGroup, e.ChatId.ToString()),
                                                    }
                                                };
                                            }));
                                            await _botClient.SendTextMessageAsync(new ChatId(chat.Id), replyText, replyMarkup: replyMarkup);
                                        }
                                        else
                                        {

                                            var replyText = "Додайте бота до групи чи каналу, у який хочете налаштувати відправку анонсів, і натисніть кнопку Оновити нижче.";
                                            var replyMarkup = new InlineKeyboardMarkup(new List<InlineKeyboardButton>()
                                            {
                                                new InlineKeyboardButton("Оновити")
                                                {
                                                    CallbackData = botContextConverter.Serialize(TelegramContext.Action.Refresh),
                                                },
                                            });
                                            await _botClient.SendTextMessageAsync(new ChatId(chat.Id), replyText, replyMarkup: replyMarkup);
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery!;
                var callbackData = callbackQuery.Data;
                var from = callbackQuery.From;
                var chatId = callbackQuery.Message.Chat.Id;
                if (callbackData != null)
                {
                    var context = botContextConverter.Deserialize(callbackData);
                    if (context.Length > 0)
                    {
                        var action = context[0];
                        if (action == TelegramContext.Action.SetUpGroup)
                        {
                            if (context.Length > 1)
                            {
                                if (Guid.TryParse(context[1], out var telegramChatId))
                                {
                                    var telegramUser = (await _context.TelegramUsers
                                        .Include(e => e.Chats)
                                            .ThenInclude(e => e.Chat)
                                        .Include(e => e.ChatSettings)
                                        .FirstOrDefaultAsync(e => e.ExternalId == from!.Id))!;

                                    var telegramChat = telegramUser.Chats.FirstOrDefault(e => e.ChatId == telegramChatId)?.Chat;
                                    if (telegramChat != null)
                                    {
                                        if (telegramChat.Settings == null)
                                        {
                                            telegramChat.Settings = new TelegramChatSettings
                                            {
                                                Owner = telegramUser,
                                            };

                                            await _context.SaveChangesAsync();
                                        }

                                        var telegramChatSettings = telegramChat.Settings;

                                        var replyText = $"Налаштування {(telegramChat.Type == ChatType.Channel ? "каналу" : "групи")} {telegramChat.Title}";
                                        var lines = new List<List<InlineKeyboardButton>>();

                                        var firstLine = new List<InlineKeyboardButton>();
                                        if (telegramChatSettings.IsEnabled)
                                        {
                                            firstLine.Add(new InlineKeyboardButton("Ввимкнути")
                                            {
                                                CallbackData = botContextConverter.Serialize(TelegramContext.Action.SettingsDisable, telegramChat.Id.ToString()),
                                            });
                                        }
                                        else
                                        {
                                            firstLine.Add(new InlineKeyboardButton("Увімкнути")
                                            {
                                                CallbackData = botContextConverter.Serialize(TelegramContext.Action.SettingsEnable, telegramChat.Id.ToString()),
                                            });
                                        }

                                        lines.Add(firstLine);

                                        var replyMarkup = new InlineKeyboardMarkup(lines);
                                        await _botClient.SendTextMessageAsync(new ChatId(chatId), replyText, replyMarkup: replyMarkup);
                                    }
                                    else
                                    {
                                        // TODO:
                                    }
                                }
                            }
                        }
                        else if (action == TelegramContext.Action.SettingsEnable)
                        {
                            if (context.Length > 1)
                            {
                                if (Guid.TryParse(context[1], out var telegramChatId))
                                {
                                    var telegramUser = (await _context.TelegramUsers
                                        .FirstOrDefaultAsync(e => e.ExternalId == from!.Id))!;
                                    if (telegramUser.AccountId != null)
                                    {
                                        var telegramChatSettings = await _context.TelegramChatSettings
                                            .Include(e => e.TelegramChat)
                                            .FirstOrDefaultAsync(e => e.TelegramChatId == telegramChatId);
                                        if (telegramChatSettings != null)
                                        {
                                            telegramChatSettings.IsEnabled = true;

                                            await _context.SaveChangesAsync();

                                            var internalEvent = new TelegramChatUpdatedEvent
                                            {
                                                AccountId = telegramUser.AccountId.Value,
                                                TelegramChatId = telegramChatId,
                                                IsEnabled = telegramChatSettings.IsEnabled,
                                            };
                                            _producer.Publish(internalEvent);

                                            var telegramChat = telegramChatSettings.TelegramChat;
                                            var replyText = $"Налаштування {(telegramChat.Type == ChatType.Channel ? "каналу" : "групи")} {telegramChat.Title}";
                                            var lines = new[]
                                            {
                                                new[]
                                                {
                                                    new InlineKeyboardButton("Ввимкнути")
                                                    {
                                                        CallbackData = botContextConverter.Serialize(TelegramContext.Action.SettingsDisable, telegramChat.Id.ToString()),
                                                    }
                                                },
                                            };

                                            var replyMarkup = new InlineKeyboardMarkup(lines);
                                            await _botClient.SendTextMessageAsync(new ChatId(chatId), replyText, replyMarkup: replyMarkup);
                                        }
                                        else
                                        {
                                            // TODO:
                                        }
                                    }
                                }
                            }
                        }
                        else if (action == TelegramContext.Action.SettingsDisable)
                        {
                            if (context.Length > 1)
                            {
                                if (Guid.TryParse(context[1], out var telegramChatId))
                                {
                                    var telegramUser = (await _context.TelegramUsers
                                        .FirstOrDefaultAsync(e => e.ExternalId == from!.Id))!;
                                    if (telegramUser.AccountId != null)
                                    {
                                        var telegramChatSettings = await _context.TelegramChatSettings
                                            .Include(e => e.TelegramChat)
                                            .FirstOrDefaultAsync(e => e.TelegramChatId == telegramChatId);
                                        if (telegramChatSettings != null)
                                        {
                                            telegramChatSettings.IsEnabled = false;

                                            await _context.SaveChangesAsync();

                                            var internalEvent = new TelegramChatUpdatedEvent
                                            {
                                                AccountId = telegramUser.AccountId.Value,
                                                TelegramChatId = telegramChatId,
                                                IsEnabled = telegramChatSettings.IsEnabled,
                                            };
                                            _producer.Publish(internalEvent);

                                            var telegramChat = telegramChatSettings.TelegramChat;
                                            var replyText = $"Налаштування {(telegramChat.Type == ChatType.Channel ? "каналу" : "групи")} {telegramChat.Title}";
                                            var lines = new[]
                                            {
                                            new[]
                                            {
                                                new InlineKeyboardButton("Увімкнути")
                                                {
                                                    CallbackData = botContextConverter.Serialize(TelegramContext.Action.SettingsEnable, telegramChat.Id.ToString()),
                                                },
                                            },
                                        };

                                            var replyMarkup = new InlineKeyboardMarkup(lines);
                                            await _botClient.SendTextMessageAsync(new ChatId(chatId), replyText, replyMarkup: replyMarkup);
                                        }
                                        else
                                        {
                                            // TODO:
                                        }
                                    }
                                }
                            }
                        }
                        else if (action == TelegramContext.Action.Refresh)
                        {
                            var telegramUser = (await _context.TelegramUsers
                                .Include(e => e.Chats)
                                    .ThenInclude(e => e.Chat)
                                .FirstOrDefaultAsync(e => e.ExternalId == from!.Id))!;
                            if (telegramUser.AccountId != null)
                            {
                                var telegramUserChats = telegramUser.Chats
                                    .Where(e => e.Chat.Type == ChatType.Group || e.Chat.Type == ChatType.Channel || e.Chat.Type == ChatType.Supergroup)
                                    .Where(e => e.Chat.SupergroupId == null)
                                    .Where(e => e.Status == ChatMemberStatus.Creator || e.Status == ChatMemberStatus.Administrator);
                                if (telegramUserChats.Any())
                                {
                                    var replyText = "Оберіть групу чи канал, до якої хотіли б налаштувати відправку анонсів:";
                                    var replyMarkup = new InlineKeyboardMarkup(telegramUserChats.Select(e =>
                                    {
                                        var chat = e.Chat;
                                        return new List<InlineKeyboardButton>()
                                        {
                                            new InlineKeyboardButton(chat.Title!)
                                            {
                                                CallbackData = botContextConverter.Serialize(TelegramContext.Action.SetUpGroup, e.ChatId.ToString()),
                                            }
                                        };
                                    }));
                                    await _botClient.SendTextMessageAsync(new ChatId(chatId), replyText, replyMarkup: replyMarkup);
                                }
                                else
                                {

                                    var replyText = "Додайте бота до групи чи каналу, у який хочете налаштувати відправку анонсів, і натисніть кнопку Оновити нижче.";
                                    var replyMarkup = new InlineKeyboardMarkup(new List<InlineKeyboardButton>()
                                    {
                                        new InlineKeyboardButton("Оновити")
                                        {
                                            CallbackData = botContextConverter.Serialize(TelegramContext.Action.Refresh),
                                        },
                                    });
                                    await _botClient.SendTextMessageAsync(new ChatId(chatId), replyText, replyMarkup: replyMarkup);
                                }
                            }
                        }
                    }
                }
            }

            return Ok();
        }

        private async Task AddOrUpdateTelegramUserAsync(TelegramChat telegramChat, User user, ChatMemberStatus? chatMemberStatus = null)
        {
            var userId = user.Id;
            var telegramUser = await _context.TelegramUsers.FirstOrDefaultAsync(e => e.ExternalId == userId);
            if (telegramUser == null)
            {
                telegramUser = new TelegramUser
                {
                    ExternalId = userId,
                };

                _context.TelegramUsers.Add(telegramUser);
            }

            telegramUser.FirstName = user.FirstName;
            telegramUser.LastName = user.LastName;
            telegramUser.Username = user.Username;

            var telegramChatMember = telegramChat.Members.FirstOrDefault(e => e.UserId == telegramUser.Id);
            if (telegramChatMember == null)
            {
                telegramChatMember = new TelegramChatMember
                {
                    User = telegramUser,
                    UserId = telegramUser.Id,
                };

                telegramChat.Members.Add(telegramChatMember);

                if (chatMemberStatus == null)
                {
                    var chatMember = await _botClient.GetChatMemberAsync(telegramChat.ExternalId, userId);
                    chatMemberStatus = chatMember.Status;
                }

                telegramChatMember.Status = chatMemberStatus.Value;
            }
        }
    }
}
