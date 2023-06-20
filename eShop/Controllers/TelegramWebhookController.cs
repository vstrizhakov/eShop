using eShop.Configurations;
using eShop.Database.Data;
using eShop.Models.TelegramWebhook;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Packaging;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.Controllers
{
    [Route(TelegramBotConfiguration.WebhookRoute)]
    [ApiController]
    public class TelegramWebhookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITelegramBotClient _botClient;
        private readonly UserManager<Database.Data.User> _userManager;

        public TelegramWebhookController(ApplicationDbContext context, ITelegramBotClient botClient, UserManager<Database.Data.User> userManager)
        {
            _context = context;
            _botClient = botClient;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
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
                        telegramChat.Members.AddRange(baseTelegramChat.Members);
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
                        var seperatorIndex = text.IndexOf(' ');
                        if (seperatorIndex == -1)
                        {
                            seperatorIndex = text.Length;
                        }

                        var command = text.Substring(0, seperatorIndex);
                        var arguments = text.Substring(seperatorIndex, text.Length - seperatorIndex);
                        switch (command)
                        {
                            case "/start":
                                if (!string.IsNullOrEmpty(arguments))
                                {
                                    var context = Encoding.UTF8.GetString(Convert.FromBase64String(arguments)).Split(',');
                                    if (context.Length > 0)
                                    {
                                        var action = context[0];

                                        if (action == StartContext.Action.RegisterClient)
                                        {
                                            if (context.Length > 1)
                                            {
                                                var userId = context[1];

                                                var telegramUser = (await _context.TelegramUsers
                                                    .Include(e => e.LinkedUser)
                                                    .FirstOrDefaultAsync(e => e.ExternalId == from!.Id))!;

                                                var linkedUser = telegramUser.LinkedUser;
                                                if (linkedUser == null)
                                                {
                                                    linkedUser = new Database.Data.User
                                                    {
                                                        UserName = telegramUser.Username ?? Guid.NewGuid().ToString(),
                                                    };

                                                    var result = await _userManager.CreateAsync(linkedUser);
                                                    if (result.Succeeded)
                                                    {
                                                        telegramUser.LinkedUser = linkedUser;
                                                    }
                                                }

                                                if (linkedUser.Id != userId)
                                                {
                                                    var provider = await _userManager.FindByIdAsync(userId);
                                                    if (provider != null)
                                                    {
                                                        linkedUser.Provider = provider;

                                                        var telegramRequest = new TelegramRequest
                                                        {
                                                            Type = TelegramRequestType.AddGroup,
                                                            User = telegramUser,
                                                        };

                                                        _context.TelegramRequests.Add(telegramRequest);

                                                        await _context.SaveChangesAsync();

                                                        var replyText = $"Hello, {telegramUser.FirstName} {telegramUser.LastName}!\nA user under the {linkedUser.Email} is now your anonce provider.\nThe next step you should set up the groups you want send anonces to.";
                                                        var replyMarkup = new ReplyKeyboardMarkup(new List<KeyboardButton>
                                                        {
                                                            new KeyboardButton("Add a group")
                                                            {
                                                                RequestChat = new KeyboardButtonRequestChat
                                                                {
                                                                    RequestId = telegramRequest.Id,
                                                                },
                                                            },
                                                        });

                                                        await _botClient.SendTextMessageAsync(new ChatId(chatId), replyText, replyMarkup: replyMarkup);
                                                    }
                                                }
                                                else
                                                {
                                                    await _botClient.SendTextMessageAsync(new ChatId(chatId), "You can't be a client of your own");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // TODO: Send default welcome message
                                    }
                                }
                                break;
                            case "/add_new_group":
                                {
                                    var telegramUser = await _context.TelegramUsers
                                        .Include(e => e.Chats)
                                            .ThenInclude(e => e.Chat)
                                        .FirstOrDefaultAsync(e => e.ExternalId == from!.Id);

                                    var telegramUserChats = telegramUser!.Chats
                                        .Where(e => e.Chat.Type == ChatType.Group || e.Chat.Type == ChatType.Channel || e.Chat.Type == ChatType.Supergroup)
                                        .Where(e => e.Chat.SupergroupId == null)
                                        .Where(e => e.Status == ChatMemberStatus.Creator || e.Status == ChatMemberStatus.Administrator);
                                    if (telegramUserChats.Any())
                                    {
                                        var replyMarkup = new InlineKeyboardMarkup(telegramUserChats.Select(e =>
                                        {
                                            var chat = e.Chat;
                                            return new List<InlineKeyboardButton>()
                                            {
                                            new InlineKeyboardButton(chat.Title!)
                                            {
                                                CallbackData = "d",
                                            }
                                            };
                                        }));
                                        await _botClient.SendTextMessageAsync(new ChatId(chat.Id), "There are the following groups or channels with you as and admin. Please, select one which you want to set up:", replyMarkup: replyMarkup);
                                    }
                                    else
                                    {
                                        await _botClient.SendTextMessageAsync(new ChatId(chat.Id), "We haven't found any group or channel with you as an admin");
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return Ok();
        }

        private async Task AddOrUpdateTelegramUserAsync(TelegramChat telegramChat, Telegram.Bot.Types.User user, ChatMemberStatus? chatMemberStatus = null)
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
