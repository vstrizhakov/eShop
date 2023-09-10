using eShop.Common.Extensions;
using eShop.Telegram.Models;
using eShop.Telegram.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Telegram.Controllers
{
    [Route("api/telegram/invitation")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        [HttpGet("{providerId}")]
        public ActionResult<GetInviteLinkResponse> GetInviteLink(
            [FromRoute] Guid providerId,
            [FromServices] ITelegramInvitationLinkGenerator linkGenerator)
        {
            // TODO: Add check for providerId
            var response = new GetInviteLinkResponse
            {
                InviteLink = linkGenerator.Generate(providerId),
            };
            return response;
        }
    }
}
