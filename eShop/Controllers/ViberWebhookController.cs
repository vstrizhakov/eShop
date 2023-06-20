using eShop.Configurations;
using eShop.Database.Data;
using eShop.ViberBot;
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
        public async Task<IActionResult> Post([FromBody] Callback callback, CancellationToken cancellationToken)
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
                    response = new Message
                    {
                        Type = MessageType.Text,
                        Text = "Welcome to the eShop Bot!",
                    };

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
                viberUser = await _context.ViberUsers.FirstOrDefaultAsync(e => e.ExternalId == senderId);
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
            }

            await _context.SaveChangesAsync();

            return Ok(response);
        }
    }
}
