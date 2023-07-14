using AutoMapper;
using eShop.Catalog.Entities;
using eShop.Catalog.Models.Compositions;
using eShop.Catalog.Repositories;
using eShop.Catalog.Services;
using eShop.Common;
using eShop.Messaging.Extensions;
using eShop.RabbitMq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.Controllers
{
    [Route("api/catalog/compositions")]
    [ApiController]
    [Authorize]
    public class CompositionsController : ControllerBase
    {
        private readonly ICompositionRepository _repository;
        private readonly IMapper _mapper;

        public CompositionsController(ICompositionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Composition>>> GetCompositions()
        {
            var ownerId = User.GetAccountId();
            var compositions = await _repository.GetCompositionsAsync(ownerId.Value);
            return Ok(compositions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Composition>> GetComposition([FromRoute] Guid id)
        {
            var composition = await _repository.GetCompositionByIdAsync(id);

            var ownerId = User.GetAccountId();
            if (composition == null || composition.OwnerId != ownerId)
            {
                return NotFound();
            }

            return composition;
        }

        [HttpPost] // TODO: Add URL validation, etc.
        public async Task<ActionResult<Composition>> PostComposition(
            [FromForm] CreateCompositionRequest request,
            [FromServices] IFileManager fileManager,
            [FromServices] IProducer producer,
            [FromServices] IPublicUriBuilder publicUriBuilder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var composition = _mapper.Map<Composition>(request);
            var userId = User.GetAccountId().Value;
            composition.OwnerId = userId;

            var image = request.Image;
            using var imageStream = image.OpenReadStream();
            var imagePath = await fileManager.SaveAsync(Path.Combine("Catalog", "Compositions", composition.Id.ToString(), "Images"), Path.GetExtension(image.FileName), imageStream);

            // TODO: Handle products` images (request.Products.Images)

            composition.Images.Add(new CompositionImage
            {
                Path = imagePath,
            });

            await _repository.CreateCompositionAsync(composition);

            var broadcastMessage = new Messaging.Models.BroadcastCompositionMessage
            {
                ProviderId = userId,
                Composition = new Messaging.Models.Composition
                {
                    Id = composition.Id,
                    Images = composition.Images.Select(e => new Uri(publicUriBuilder.Path(e.Path))),
                    Products = composition.Products.Select(e => new Messaging.Models.Product
                    {
                        Name = e.Name,
                        Url = new Uri(e.Url),
                        Price = e.Prices.FirstOrDefault().Value,
                    }),
                },
            };
            producer.Publish(broadcastMessage);

            return CreatedAtAction("GetComposition", new { id = composition.Id }, composition);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComposition(
            [FromRoute] Guid id,
            [FromServices] IFileManager fileManager)
        {
            var composition = await _repository.GetCompositionByIdAsync(id);

            var ownerId = User.GetAccountId();
            if (composition == null || composition.OwnerId != ownerId)
            {
                return NotFound();
            }

            foreach (var image in composition.Images)
            {
                await fileManager.DeleteAsync(image.Path);
            }

            await _repository.DeleteCompositionAsync(composition);

            return NoContent();
        }
    }
}
