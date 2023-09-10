using eShop.Common.Extensions;
using eShop.Viber.Models;
using eShop.Viber.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Viber.Controllers
{
    [Route("api/viber/invitation")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        [HttpGet("{providerId}")]
        public ActionResult<GetInviteLinkResponse> GetInviteLink(
            [FromRoute] Guid providerId,
            [FromServices] IViberInvitationLinkGenerator viberInvitationLinkGenerator)
        {
            // TODO: Add check for providerId
            var response = new GetInviteLinkResponse
            {
                InviteLink = viberInvitationLinkGenerator.Generate(providerId),
            };
            return response;
        }
    }
}
