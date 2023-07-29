using eShop.Common.Extensions;
using eShop.Telegram.Models;
using eShop.Telegram.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Telegram.Controllers
{
    [Route("api/telegram/invitation")]
    [ApiController]
    [Authorize]
    public class InvitationController : ControllerBase
    {
        [HttpGet]
        public ActionResult<GetInviteLinkResponse> GetInviteLink(
            [FromServices] ITelegramInvitationLinkGenerator linkGenerator)
        {
            var providerId = User.GetAccountId().Value;
            var response = new GetInviteLinkResponse
            {
                InviteLink = linkGenerator.Generate(providerId),
            };
            return response;
        }
    }
}
