using eShop.Bots.Common;
using eShop.Common.Extensions;
using eShop.Telegram.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace eShop.Telegram.Controllers
{
    [Route("api/telegram/invitation")]
    [ApiController]
    [Authorize]
    public class InvitationController : ControllerBase
    {
        [HttpGet]
        public ActionResult<GetInviteLinkResponse> GetInviteLink(
            [FromServices] IBotContextConverter botContextConverter,
            [FromServices] IOptions<TelegramBotConfiguration> telegramBotConfiguration)
        {
            var providerId = User.GetAccountId();
            var context = botContextConverter.Serialize(TelegramContext.Action.RegisterClient, providerId.ToString());

            var response = new GetInviteLinkResponse
            {
                InviteLink = QueryHelpers.AddQueryString($"https://t.me/{telegramBotConfiguration.Value.Username}", "start", context),
            };

            return response;
        }
    }
}
