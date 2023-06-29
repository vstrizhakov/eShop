using eShop.Configurations;
using eShop.Database.Data;
using eShop.Models;
using eShop.Services;
using eShop.ViberBot;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace eShop.Controllers
{
    [Route(ViberBotConfiguration.WebhookRoute)]
    [ApiController]
    public class ViberWebhookController : ControllerBase
    {
        private readonly IViberBotClient _botClient;
        private readonly ApplicationDbContext _context;

        public ViberWebhookController(IViberBotClient botClient, ApplicationDbContext context)
        {
            _botClient = botClient;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] Callback callback,
            [FromServices] ITelegramContextConverter telegramContextConverter,
            [FromServices] UserManager<Database.Data.User> userManager,
            CancellationToken cancellationToken)
        {
            object? response = null;
            ViberBot.User? sender = null;
            bool? isSubscribed = null;
            string? senderId = null;

            switch (callback.Event)
            {
                case EventType.Webhook:
                    Debug.WriteLine($"Webhook connected");
                    break;
                case EventType.ConversationStarted:
                    sender = callback.User;
                    isSubscribed = callback.Subscribed;
                    break;
                case EventType.Subscribed:
                    sender = callback.User;
                    isSubscribed = true;
                    break;
                case EventType.Unsubscribed:
                    senderId = callback.UserId;
                    isSubscribed = false;
                    break;
                case EventType.Message:
                    sender = callback.Sender;
                    isSubscribed = true;
                    break;
                default:
                    break;
            }

            if (senderId == null && sender != null)
            {
                senderId = sender.Id;
            }

            ViberUser? viberUser = null;
            if (senderId != null)
            {
                viberUser = await _context.ViberUsers
                    .Include(e => e.Owner)
                        .ThenInclude(e => e.ViberChatSettings)
                    .FirstOrDefaultAsync(e => e.ExternalId == senderId);
            }

            if (sender != null && senderId != null)
            {
                Debug.Assert(sender.Name != null);

                if (viberUser == null)
                {
                    viberUser = new ViberUser
                    {
                        ExternalId = senderId,
                    };

                    _context.ViberUsers.Add(viberUser);
                }

                viberUser.Name = sender.Name;
            }

            if (viberUser != null)
            {
                Debug.Assert(isSubscribed.HasValue);

                viberUser.IsSubcribed = isSubscribed.Value;

                await _context.SaveChangesAsync();

                if (callback.Event == EventType.ConversationStarted)
                {
                    var callbackData = callback.Context;
                    if (!string.IsNullOrEmpty(callbackData))
                    {
                        var context = telegramContextConverter.Deserialize(callbackData);
                        if (context.Length > 0)
                        {
                            var action = context[0];
                            if (action == ViberContext.RegisterClient)
                            {
                                if (context.Length > 1)
                                {
                                    var providerId = context[1];

                                    var owner = viberUser.Owner;
                                    if (owner == null)
                                    {
                                        owner = new Database.Data.User
                                        {
                                            UserName = Guid.NewGuid().ToString(),
                                            ViberChatSettings = new ViberChatSettings
                                            {
                                                ViberUser = viberUser,
                                            },
                                            ViberUser = viberUser,
                                        };

                                        var result = await userManager.CreateAsync(owner);
                                    }

                                    if (owner.Id != providerId)
                                    {
                                        var provider = await userManager.FindByIdAsync(providerId);
                                        if (provider != null)
                                        {
                                            owner.Provider = provider;

                                            await _context.SaveChangesAsync();

                                            var replyText = $"{owner.Email} встановлений як Ваш постачальник анонсів.";

                                            response = new Message
                                            {
                                                Type = MessageType.Text,
                                                MinApiVersion = 7,
                                                Text = replyText,
                                                Keyboard = new Keyboard
                                                {
                                                    Buttons = new[]
                                                    {
                                                        new Button
                                                        {
                                                            Rows = 1,
                                                            Text = "Увімкнути",
                                                            ActionBody = telegramContextConverter.Serialize(ViberContext.SettingsEnable),
                                                        },
                                                    },
                                                },
                                            };
                                        }
                                    }
                                    else
                                    {
                                        response = new Message
                                        {
                                            Type = MessageType.Text,
                                            Text = "You can't be a client of your own",
                                        };
                                    }
                                }
                            }
                        }
                    }
                }
                else if (callback.Event == EventType.Message)
                {
                    var data = callback.Message!.Text;
                    if (!string.IsNullOrEmpty(data))
                    {
                        var context = Array.Empty<string>();
                        try
                        {
                            context = telegramContextConverter.Deserialize(data);
                        }
                        catch
                        {
                        }
                        if (context.Length > 0)
                        {
                            var action = context[0];
                            if (action == ViberContext.SettingsEnable)
                            {
                                var owner = viberUser.Owner;
                                if (owner != null)
                                {
                                    owner.ViberChatSettings.IsEnabled = true;

                                    await _context.SaveChangesAsync();

                                    var replyText = "Надсилання анонсів увімкнено";
                                    var keyboard = new Keyboard
                                    {
                                        Buttons = new[]
                                        {
                                            new Button
                                            {
                                                Rows = 1,
                                                Text = "Ввимкнути",
                                                ActionBody = telegramContextConverter.Serialize(ViberContext.SettingsDisable),
                                            },
                                        },
                                    };
                                    await _botClient.SendTextMessageAsync(viberUser.ExternalId, null, replyText, keyboard: keyboard);
                                }
                            }
                            else if (action == ViberContext.SettingsDisable)
                            {
                                var owner = viberUser.Owner;
                                if (owner != null)
                                {
                                    owner.ViberChatSettings.IsEnabled = false;

                                    await _context.SaveChangesAsync();

                                    var replyText = "Надсилання анонсів ввимкнено";
                                    var keyboard = new Keyboard
                                    {
                                        Buttons = new[]
                                        {
                                            new Button
                                            {
                                                Rows = 1,
                                                Text = "Увікмнути",
                                                ActionBody = telegramContextConverter.Serialize(ViberContext.SettingsEnable),
                                            },
                                        },
                                    };
                                    await _botClient.SendTextMessageAsync(viberUser.ExternalId, null, replyText, keyboard: keyboard);
                                }
                            }
                            else if (action == ViberContext.Settings)
                            {
                                var owner = viberUser.Owner;
                                if (owner != null)
                                {
                                    var isEnabled = owner.ViberChatSettings.IsEnabled;

                                    await _context.SaveChangesAsync();

                                    var replyText = isEnabled ? "Надсилання анонсів увімкнено" : "Надсилання анонсів ввимкнено";
                                    var keyboard = new Keyboard
                                    {
                                        Buttons = new[]
                                        {
                                            new Button
                                            {
                                                Rows = 1,
                                                Text = isEnabled ? "Ввимкнути" : "Увікмнути",
                                                ActionBody = telegramContextConverter.Serialize(isEnabled ? ViberContext.SettingsDisable : ViberContext.SettingsEnable),
                                            },
                                        },
                                    };
                                    await _botClient.SendTextMessageAsync(viberUser.ExternalId, null, replyText, keyboard: keyboard);
                                }
                            }
                        }
                    }
                }
            }

            return Ok(response);
        }
    }
}
