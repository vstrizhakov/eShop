using eShop.Bots.Common;
using eShop.Viber.Entities;
using eShop.Viber.Models;
using eShop.Viber.Repositories;
using eShop.ViberBot;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace eShop.Viber.Controllers
{
    [Route(ViberBotConfiguration.WebhookRoute)]
    [ApiController]
    public class ViberWebhookController : ControllerBase
    {
        private readonly IViberBotClient _botClient;
        private readonly IViberUserRepository _repository;

        public ViberWebhookController(IViberBotClient botClient, IViberUserRepository repository)
        {
            _botClient = botClient;
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] Callback callback,
            [FromServices] IBotContextConverter botContextConverter,
            [FromServices] Messaging.IProducer producer,
            CancellationToken cancellationToken)
        {
            object? response = null;
            User? sender = null;
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
                viberUser = await _repository.GetViberUserByExternalIdAsync(senderId);
            }

            if (sender != null && senderId != null)
            {
                Debug.Assert(sender.Name != null);

                if (viberUser == null)
                {
                    viberUser = new ViberUser
                    {
                        ExternalId = senderId,
                        Name = sender.Name,
                        ChatSettings = new ViberChatSettings(),
                    };

                    await _repository.CreateViberUserAsync(viberUser);
                }
                else
                {
                    viberUser.Name = sender.Name;

                    await _repository.UpdateViberUserAsync(viberUser);
                }
            }

            if (viberUser != null)
            {
                Debug.Assert(isSubscribed.HasValue);

                viberUser.IsSubcribed = isSubscribed.Value;

                await _repository.UpdateViberUserAsync(viberUser);

                if (callback.Event == EventType.ConversationStarted)
                {
                    var callbackData = callback.Context;
                    if (!string.IsNullOrEmpty(callbackData))
                    {
                        var context = botContextConverter.Deserialize(callbackData);
                        if (context.Length > 0)
                        {
                            var action = context[0];
                            if (action == ViberContext.RegisterClient)
                            {
                                if (context.Length > 1)
                                {
                                    if (Guid.TryParse(context[1], out var providerId))
                                    {
                                        if (viberUser.AccountId == null)
                                        {
                                            viberUser.RegistrationProviderId = providerId;

                                            await _repository.UpdateViberUserAsync(viberUser);

                                            response = new Message
                                            {
                                                Type = MessageType.Text,
                                                Text = "Для завершення реєстрації надішліть свій номер телефону, будь ласка",
                                                MinApiVersion = 7,
                                                Keyboard = new Keyboard
                                                {
                                                    Buttons = new[]
                                                    {
                                                        new Button
                                                        {
                                                            Text = "Відправити номер телефону",
                                                            ActionType = "share-phone",
                                                            ActionBody = string.Empty,
                                                        },
                                                    },
                                                },
                                            };
                                        }
                                        else
                                        {
                                            response = new Message
                                            {
                                                Type = MessageType.Text,
                                                Text = "Ви вже зареєстровані та маєтє встановленного постачальника анонсів",
                                            };
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (callback.Event == EventType.Message)
                {
                    var message = callback.Message!;
                    if (message.Type == MessageType.Text)
                    {
                        var data = message.Text;
                        if (!string.IsNullOrEmpty(data))
                        {
                            var context = Array.Empty<string>();
                            try
                            {
                                context = botContextConverter.Deserialize(data);
                            }
                            catch
                            {
                            }
                            if (context.Length > 0)
                            {
                                var action = context[0];
                                if (action == ViberContext.SettingsEnable)
                                {
                                    if (viberUser.AccountId != null)
                                    {
                                        await _repository.UpdateChatSettingsAsync(viberUser, true);

                                        var internalMessage = new Messaging.Models.ViberChatUpdatedEvent
                                        {
                                            AccountId = viberUser.AccountId.Value,
                                            ViberUserId = viberUser.Id,
                                            IsEnabled = true,
                                        };

                                        producer.Publish(internalMessage);

                                        var replyText = "Надсилання анонсів увімкнено";
                                        var keyboard = new Keyboard
                                        {
                                            Buttons = new[]
                                            {
                                            new Button
                                            {
                                                Rows = 1,
                                                Text = "Ввимкнути",
                                                ActionBody = botContextConverter.Serialize(ViberContext.SettingsDisable),
                                            },
                                        },
                                        };
                                        await _botClient.SendTextMessageAsync(viberUser.ExternalId, null, replyText, keyboard: keyboard);
                                    }
                                }
                                else if (action == ViberContext.SettingsDisable)
                                {
                                    if (viberUser.AccountId != null)
                                    {
                                        await _repository.UpdateChatSettingsAsync(viberUser, false);

                                        var internalMessage = new Messaging.Models.ViberChatUpdatedEvent
                                        {
                                            AccountId = viberUser.AccountId.Value,
                                            ViberUserId = viberUser.Id,
                                            IsEnabled = false,
                                        };

                                        producer.Publish(internalMessage);

                                        var replyText = "Надсилання анонсів ввимкнено";
                                        var keyboard = new Keyboard
                                        {
                                            Buttons = new[]
                                            {
                                            new Button
                                            {
                                                Rows = 1,
                                                Text = "Увікмнути",
                                                ActionBody = botContextConverter.Serialize(ViberContext.SettingsEnable),
                                            },
                                        },
                                        };
                                        await _botClient.SendTextMessageAsync(viberUser.ExternalId, null, replyText, keyboard: keyboard);
                                    }
                                }
                                else if (action == ViberContext.Settings)
                                {
                                    if (viberUser.AccountId != null)
                                    {
                                        var isEnabled = viberUser.ChatSettings.IsEnabled;

                                        var replyText = isEnabled ? "Надсилання анонсів увімкнено" : "Надсилання анонсів ввимкнено";
                                        var keyboard = new Keyboard
                                        {
                                            Buttons = new[]
                                            {
                                            new Button
                                            {
                                                Rows = 1,
                                                Text = isEnabled ? "Ввимкнути" : "Увікмнути",
                                                ActionBody = botContextConverter.Serialize(isEnabled ? ViberContext.SettingsDisable : ViberContext.SettingsEnable),
                                            },
                                        },
                                        };
                                        await _botClient.SendTextMessageAsync(viberUser.ExternalId, null, replyText, keyboard: keyboard);
                                    }
                                }
                            }
                        }
                    }
                    else if (message.Type == MessageType.Contact)
                    {
                        var contact = message.Contact!;

                        var providerId = viberUser.RegistrationProviderId;
                        if (providerId.HasValue)
                        {
                            var phoneNumber = contact.PhoneNumber;

                            viberUser.PhoneNumber = phoneNumber;
                            viberUser.RegistrationProviderId = null;

                            await _repository.UpdateViberUserAsync(viberUser);

                            var internalMessage = new Messaging.Models.Viber.RegisterViberUserRequest
                            {
                                ViberUserId = viberUser.Id,
                                ProviderId = providerId.Value,
                                Name = viberUser.Name,
                                PhoneNumber = phoneNumber,
                            };

                            producer.Publish(internalMessage);
                        }
                    }
                }
            }

            return Ok(response);
        }
    }
}
