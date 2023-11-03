using AutoMapper;
using eShop.Catalog.Entities;
using eShop.Catalog.Services;
using eShop.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.Controllers
{
    [Route("api/catalog/announces")]
    [ApiController]
    [Authorize]
    public class AnnouncesController : ControllerBase
    {
        private readonly IAnnouncesService _announceService;
        private readonly IMapper _mapper;

        public AnnouncesController(IAnnouncesService announceService, IMapper mapper)
        {
            _announceService = announceService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Announces.Announce>>> GetAnnounces()
        {
            var ownerId = User.GetAccountId();
            var announces = await _announceService.GetAnnouncesAsync(ownerId.Value);
            var response = _mapper.Map<IEnumerable<Models.Announces.Announce>>(announces);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Announces.Announce>> GetAnnounce([FromRoute] Guid id)
        {
            var announce = await _announceService.GetAnnounceAsync(id);

            var ownerId = User.GetAccountId();
            if (announce == null || announce.OwnerId != ownerId)
            {
                return NotFound();
            }

            var response = _mapper.Map<Models.Announces.Announce>(announce);
            return response;
        }

        [HttpPost] // TODO: Add URL validation, etc.
        public async Task<ActionResult<Models.Announces.Announce>> CreateAnnounce(
            [FromForm] Models.Announces.CreateAnnounceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var announce = _mapper.Map<Announce>(request);
            var userId = User.GetAccountId().Value;
            announce.OwnerId = userId;

            await _announceService.CreateAnnounceAsync(announce, request.Image);

            var response = _mapper.Map<Models.Announces.Announce>(announce);
            return CreatedAtAction(nameof(GetAnnounce), new { id = announce.Id }, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnounce([FromRoute] Guid id)
        {
            var announce = await _announceService.GetAnnounceAsync(id);

            var ownerId = User.GetAccountId();
            if (announce == null || announce.OwnerId != ownerId)
            {
                return NotFound();
            }

            await _announceService.DeleteAnnounceAsync(announce);

            return NoContent();
        }
    }
}
