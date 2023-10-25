using eShop.ViberBot;
using eShop.ViberBot.Framework;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Viber.Controllers
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
