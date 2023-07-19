using AutoMapper;
using eShop.Catalog.Entities;
using eShop.Catalog.Services;
using eShop.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.Controllers
{
    [Route("api/catalog/compositions")]
    [ApiController]
    [Authorize]
    public class CompositionsController : ControllerBase
    {
        private readonly ICompositionService _compositionService;
        private readonly IMapper _mapper;

        public CompositionsController(ICompositionService compositionService, IMapper mapper)
        {
            _compositionService = compositionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Compositions.Composition>>> GetCompositions()
        {
            var ownerId = User.GetAccountId();
            var compositions = await _compositionService.GetCompositionsAsync(ownerId.Value);
            var response = _mapper.Map<IEnumerable< Models.Compositions.Composition>>(compositions);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Compositions.Composition>> GetComposition([FromRoute] Guid id)
        {
            var composition = await _compositionService.GetCompositionAsync(id);

            var ownerId = User.GetAccountId();
            if (composition == null || composition.OwnerId != ownerId)
            {
                return NotFound();
            }

            var response = _mapper.Map<Models.Compositions.Composition>(composition);
            return response;
        }

        [HttpPost] // TODO: Add URL validation, etc.
        public async Task<ActionResult<Models.Compositions.Composition>> CreateComposition(
            [FromForm] Models.Compositions.CreateCompositionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var composition = _mapper.Map<Composition>(request);
            var userId = User.GetAccountId().Value;
            composition.OwnerId = userId;

            await _compositionService.CreateCompositionAsync(composition, request.Image);

            var response = _mapper.Map<Models.Compositions.Composition>(composition);
            return CreatedAtAction("GetComposition", new { id = composition.Id }, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComposition([FromRoute] Guid id)
        {
            var composition = await _compositionService.GetCompositionAsync(id);

            var ownerId = User.GetAccountId();
            if (composition == null || composition.OwnerId != ownerId)
            {
                return NotFound();
            }

            await _compositionService.DeleteCompositionAsync(composition);

            return NoContent();
        }
    }
}
