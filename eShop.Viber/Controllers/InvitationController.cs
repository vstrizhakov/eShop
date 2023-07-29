using eShop.Common.Extensions;
using eShop.Viber.Models;
using eShop.Viber.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Viber.Controllers
{
    [Route("api/viber/invitation")]
    [ApiController]
    [Authorize]
    public class InvitationController : ControllerBase
    {
        [HttpGet]
        public ActionResult<GetInviteLinkResponse> GetInviteLink(
            [FromServices] IViberInvitationLinkGenerator viberInvitationLinkGenerator)
        {
            var providerId = User.GetAccountId().Value;
            var response = new GetInviteLinkResponse
            {
                InviteLink = viberInvitationLinkGenerator.Generate(providerId),
            };
            return response;
        }
    }
}
