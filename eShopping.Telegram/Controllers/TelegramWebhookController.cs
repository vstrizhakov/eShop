using eShopping.TelegramFramework;
using eShopping.Telegram;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace eShopping.Telegram.Controllers
{
    [Route(TelegramBotConfiguration.WebhookRoute)]
    [ApiController]
    public class TelegramWebhookController : ControllerBase
    {
        [HttpPost]
        public IActionResult Webhook(Update update, IUpdatePublisher updatePublisher)
        {
            updatePublisher.Publish(update);

            return Ok();
        }
    }
}
