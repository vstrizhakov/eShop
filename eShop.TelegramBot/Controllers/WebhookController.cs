using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShop.TelegramBot.Controllers
{
    [Route(BotConfiguration.WebhookRoute)]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update, [FromServices] ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, message.Text, cancellationToken: cancellationToken);
                }
            }
            return Ok();
        }
    }
}
