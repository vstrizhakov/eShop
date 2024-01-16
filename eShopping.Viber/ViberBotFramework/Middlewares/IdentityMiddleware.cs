using eShopping.Viber.Entities;
using eShopping.ViberBot;
using eShopping.ViberBot.Framework;
using eShopping.Viber.Entities;
using eShopping.Viber.Repositories;
using System.Diagnostics;

namespace eShopping.Viber.ViberBotFramework.Middlewares
{
    public class IdentityMiddleware : IViberMiddleware
    {
        private readonly IViberUserRepository _viberUserRepository;

        public IdentityMiddleware(IViberUserRepository viberUserRepository)
        {
            _viberUserRepository = viberUserRepository;
        }

        public async Task ProcessAsync(Callback callback)
        {
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
                viberUser = await _viberUserRepository.GetViberUserByExternalIdAsync(senderId);
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

                    await _viberUserRepository.CreateViberUserAsync(viberUser);
                }
                else
                {
                    viberUser.Name = sender.Name;

                    await _viberUserRepository.UpdateViberUserAsync(viberUser);
                }
            }

            if (viberUser != null)
            {
                Debug.Assert(isSubscribed.HasValue);

                viberUser.IsSubcribed = isSubscribed.Value;

                await _viberUserRepository.UpdateViberUserAsync(viberUser);
            }
        }
    }
}
