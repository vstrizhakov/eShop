using AutoMapper;
using eShopping.Bots.Links;
using eShopping.Distribution.Models;
using eShopping.Distribution.Services;
using Microsoft.AspNetCore.Mvc;

namespace eShopping.Distribution.Controllers
{
    [Route("api/distribution/announcers")]
    [ApiController]
    public class AnnouncersController : ControllerBase
    {
        [HttpGet("{announcerId}/invitation")]
        public async Task<ActionResult<GetAnnouncerInvitationResponse>> GetAnnouncerInvitation(
            [FromRoute] Guid announcerId,
            [FromServices] ITelegramLinkGenerator telegramLinkGenerator,
            [FromServices] IViberLinkGenerator viberLinkGenerator,
            [FromServices] IAccountService accountService,
            [FromServices] IMapper mapper)
        {
            var announcer = await accountService.GetAccountByIdAsync(announcerId);
            if (announcer == null)
            {
                return NotFound();
            }

            var announcerIdArg = announcerId.ToString();
            var response = new GetAnnouncerInvitationResponse
            {
                Announcer = mapper.Map<Account>(announcer),
                Links = new InvitationLinks
                {
                    Telegram = telegramLinkGenerator.Generate("sca", announcerIdArg),
                    Viber = viberLinkGenerator.Generate("sca", announcerIdArg),
                },
            };
            return response;
        }
    }
}
