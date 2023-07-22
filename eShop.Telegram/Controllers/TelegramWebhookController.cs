using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Telegram.Entities;
using eShop.Telegram.Models;
using eShop.Telegram.Repositories;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ITelegramUserRepository _telegramUserRepository;
        private readonly ITelegramChatRepository _telegramChatRepository;
        private readonly ITelegramBotClient _botClient;
        private readonly IProducer _producer;

        public TelegramWebhookController(
            ITelegramUserRepository telegramUserRepository,
            ITelegramChatRepository telegramChatRepository,
            ITelegramBotClient botClient,
            IProducer producer)
        {
            _telegramUserRepository = telegramUserRepository;
            _telegramChatRepository = telegramChatRepository;
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
                var telegramChat = await _telegramChatRepository.GetTelegramChatByExternalIdAsync(chatId);
                if (telegramChat == null)
                {
                    telegramChat = new TelegramChat
                    {
                        Type = chat.Type,
                        Title = chat.Title,
                        ExternalId = chatId,
                    };

                    await _telegramChatRepository.CreateTelegramChatAsync(telegramChat);

                    if (chat.Type == ChatType.Group || chat.Type == ChatType.Supergroup || chat.Type == ChatType.Channel)
                    {
                        var chatAdministrators = await _botClient.GetChatAdministratorsAsync(new ChatId(chatId));

                        foreach (var chatAdministrator in chatAdministrators)
                        {
                            await AddOrUpdateTelegramUserAsync(telegramChat, chatAdministrator.User, chatAdministrator.Status);
                        }

                        await _telegramChatRepository.UpdateTelegramChatAsync(telegramChat);
                    }
                }
                else
                {
                    telegramChat.Type = chat.Type;
                    telegramChat.Title = chat.Title;

                    await _telegramChatRepository.UpdateTelegramChatAsync(telegramChat);
                }

                if (message.Type == MessageType.MigratedFromGroup)
                {
                    var baseTelegramChat = await _telegramChatRepository.GetTelegramChatByExternalIdAsync(message.MigrateFromChatId!.Value);
                    if (baseTelegramChat != null)
                    {
                        baseTelegramChat.Supergroup = telegramChat;
                        foreach (var member in baseTelegramChat.Members)
                        {
                            telegramChat.Members.Add(member);
                        }

                        await _telegramChatRepository.UpdateTelegramChatAsync(telegramChat);
                    }
                }

                var from = message.From;
                if (from != null)
                {
                    await AddOrUpdateTelegramUserAsync(telegramChat, from);

                    await _telegramChatRepository.UpdateTelegramChatAsync(telegramChat);
                }

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
                                                    var telegramUser = await _telegramUserRepository.GetTelegramUserByExternalIdAsync(from!.Id);
                                                    if (telegramUser!.AccountId == null)
                                                    {
                                                        telegramUser.RegistrationProviderId = providerId;

                                                        await _telegramUserRepository.UpdateTelegramUserAsync(telegramUser);

                                                        var replyText = "Для завершення реєстрації надішліть свій номер телефону, будь ласка";
                                                        var replyMarkup = new ReplyKeyboardMarkup(new KeyboardButton("Відправити номер телефону")
                                                        {
                                                            RequestContact = true,
                                                        });
                                                        await _botClient.SendTextMessageAsync(new ChatId(chatId), replyText, replyMarkup: replyMarkup);
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
                                    var telegramUser = await _telegramUserRepository.GetTelegramUserByExternalIdAsync(from!.Id);
                                    if (telegramUser!.AccountId != null)
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
                    else if (message.Type == MessageType.Contact)
                    {
                        var contact = message.Contact!;

                        var telegramUser = await _telegramUserRepository.GetTelegramUserByExternalIdAsync(from!.Id);
                        var providerId = telegramUser.RegistrationProviderId;
                        if (providerId != null)
                        {
                            var phoneNumber = contact.PhoneNumber;

                            telegramUser.PhoneNumber = phoneNumber;
                            telegramUser.RegistrationProviderId = null;

                            await _telegramUserRepository.UpdateTelegramUserAsync(telegramUser);

                            var internalMessage = new Messaging.Models.TelegramUserCreateAccountRequestMessage
                            {
                                TelegramUserId = telegramUser.Id,
                                FirstName = telegramUser.FirstName,
                                LastName = telegramUser.LastName,
                                PhoneNumber = phoneNumber,
                                ProviderId = providerId.Value,
                            };
                            _producer.Publish(internalMessage);
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
                                    var telegramUser = await _telegramUserRepository.GetTelegramUserByExternalIdAsync(from!.Id);
                                    var telegramChat = telegramUser!.Chats.FirstOrDefault(e => e.ChatId == telegramChatId)?.Chat;
                                    if (telegramChat != null)
                                    {
                                        if (telegramChat.Settings == null)
                                        {
                                            telegramChat.Settings = new TelegramChatSettings
                                            {
                                                Owner = telegramUser,
                                            };

                                            await _telegramUserRepository.UpdateTelegramUserAsync(telegramUser);
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
                                    var telegramUser = await _telegramUserRepository.GetTelegramUserByExternalIdAsync(from!.Id);
                                    if (telegramUser!.AccountId != null)
                                    {
                                        var telegramChat = await _telegramChatRepository.GetTelegramChatByIdAsync(telegramChatId);
                                        if (telegramChat != null)
                                        {
                                            var telegramChatSettings = telegramChat.Settings;
                                            telegramChatSettings.IsEnabled = true;

                                            await _telegramChatRepository.UpdateTelegramChatAsync(telegramChat);

                                            var internalEvent = new Messaging.Models.TelegramChatUpdatedEvent
                                            {
                                                AccountId = telegramUser.AccountId.Value,
                                                TelegramChatId = telegramChatId,
                                                IsEnabled = telegramChatSettings.IsEnabled,
                                            };
                                            _producer.Publish(internalEvent);

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
                                    var telegramUser = await _telegramUserRepository.GetTelegramUserByExternalIdAsync(from!.Id);
                                    if (telegramUser!.AccountId != null)
                                    {
                                        var telegramChat = await _telegramChatRepository.GetTelegramChatByIdAsync(telegramChatId);
                                        if (telegramChat != null)
                                        {
                                            var telegramChatSettings = telegramChat.Settings;
                                            telegramChatSettings.IsEnabled = false;

                                            await _telegramChatRepository.UpdateTelegramChatAsync(telegramChat);

                                            var internalEvent = new Messaging.Models.TelegramChatUpdatedEvent
                                            {
                                                AccountId = telegramUser.AccountId.Value,
                                                TelegramChatId = telegramChatId,
                                                IsEnabled = telegramChatSettings.IsEnabled,
                                            };
                                            _producer.Publish(internalEvent);

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
                            var telegramUser = await _telegramUserRepository.GetTelegramUserByExternalIdAsync(from!.Id);
                            if (telegramUser!.AccountId != null)
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
            var telegramUser = await _telegramUserRepository.GetTelegramUserByExternalIdAsync(userId);
            if (telegramUser == null)
            {
                telegramUser = new TelegramUser
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    ExternalId = userId,
                };

                await _telegramUserRepository.CreateTelegramUserAsync(telegramUser);
            }
            else
            {
                telegramUser.FirstName = user.FirstName;
                telegramUser.LastName = user.LastName;
                telegramUser.Username = user.Username;

                await _telegramUserRepository.UpdateTelegramUserAsync(telegramUser);
            }

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
