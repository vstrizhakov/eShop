using eShop.Bots.Common;
using eShop.Common;
using eShop.Viber.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace eShop.Viber.Controllers
{
    [Route("api/viber/invitation")]
    [ApiController]
    [Authorize]
    public class InvitationController : ControllerBase
    {
        [HttpGet]
        public ActionResult<GetInviteLinkResponse> GetInviteLink(
            [FromServices] IBotContextConverter botContextConverter,
            [FromServices] IOptions<ViberBotConfiguration> viberBotConfiguration)
        {
            var providerId = User.GetAccountId();
            var viberContext = botContextConverter.Serialize(ViberContext.RegisterClient, providerId.ToString());

            var response = new GetInviteLinkResponse
            {
                InviteLink = QueryHelpers.AddQueryString($"viber://pa", new Dictionary<string, string?>()
                {
                    { "chatURI", viberBotConfiguration.Value.ChatUrl },
                    { "context", viberContext },
                }),
            };

            return response;
        }
    }
}
