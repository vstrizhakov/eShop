using eShop.Bots.Links;
using eShop.Viber.Models;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Distribution.Controllers
{
    [Route("api/distribution/announcers")]
    [ApiController]
    public class AnnouncersController : ControllerBase
    {
        [HttpGet("{announcerId}/invitation")]
        public async Task<ActionResult<GetAnnouncerInvitationResponse>> GetAnnouncerInvitation(
            [FromRoute] Guid announcerId,
            [FromServices] ITelegramLinkGenerator telegramLinkGenerator,
            [FromServices] IViberLinkGenerator viberLinkGenerator)
        {
            // TODO: Add check for providerId if it exists

            var announcerIdArg = announcerId.ToString();
            var response = new GetAnnouncerInvitationResponse
            {
                Telegram = telegramLinkGenerator.Generate("rc", announcerIdArg),
                Viber = viberLinkGenerator.Generate("rc", announcerIdArg)
            };
            return response;
        }
    }
}
