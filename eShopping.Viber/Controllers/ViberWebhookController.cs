using eShopping.ViberBot;
using eShopping.ViberBot.Framework;
using eShopping.Viber;
using Microsoft.AspNetCore.Mvc;

namespace eShopping.Viber.Controllers
{
    [Route(ViberBotConfiguration.WebhookRoute)]
    [ApiController]
    public class ViberWebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] Callback callback,
            [FromServices] IPipeline pipeline)
        {
            var response = await pipeline.HandleAsync(callback);

            return Ok(response);
        }
    }
}
