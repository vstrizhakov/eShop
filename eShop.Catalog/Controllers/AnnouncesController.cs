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
        private readonly IShopService _shopService;

        public AnnouncesController(IAnnouncesService announceService, IMapper mapper, IShopService shopService)
        {
            _announceService = announceService;
            _mapper = mapper;
            _shopService = shopService;
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
            var ownerId = User.GetAccountId();
            var announce = await _announceService.GetAnnounceAsync(id, ownerId.Value);
            if (announce == null || announce.OwnerId != ownerId)
            {
                return NotFound();
            }

            var response = _mapper.Map<Models.Announces.Announce>(announce);
            return response;
        }

        [HttpPost] // TODO: Add URL validation, etc. | Validation URL already added
        public async Task<ActionResult<Models.Announces.Announce>> CreateAnnounce(
            [FromForm] Models.Announces.CreateAnnounceRequest request,
            [FromServices] ICurrencyService currencyService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var shop = await _shopService.GetShopAsync(request.ShopId);
            if (shop == null)
            {
                return BadRequest();
            }

            var currencyIds = request.Products.Select(p => p.Price.CurrencyId).Distinct();
            var currencies = await currencyService.GetCurrenciesAsync(currencyIds);
            if (currencies.Count() != currencyIds.Count())
            {
                return BadRequest();
            }

            var announce = _mapper.Map<Announce>(request);
            var userId = User.GetAccountId().Value;
            announce.OwnerId = userId;
            announce.Shop = shop.GenerateEmbedded();

            for (int i = 0; i < request.Products.Count(); i++)
            {
                var requestProduct = request.Products.ElementAt(i);
                var product = announce.Products.ElementAt(i);

                var requestCurrencyId = requestProduct.Price.CurrencyId;
                var currency = currencies.FirstOrDefault(e => e.Id == requestCurrencyId);
                var productPrice = product.Prices.FirstOrDefault();
                productPrice.Currency = currency.GenerateEmbedded();
            }

            await _announceService.CreateAnnounceAsync(announce, request.Image);

            var response = _mapper.Map<Models.Announces.Announce>(announce);
            return CreatedAtAction(nameof(GetAnnounce), new { id = announce.Id }, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnounce([FromRoute] Guid id)
        {
            var ownerId = User.GetAccountId();
            var announce = await _announceService.GetAnnounceAsync(id, ownerId.Value);
            if (announce == null || announce.OwnerId != ownerId)
            {
                return NotFound();
            }

            await _announceService.DeleteAnnounceAsync(announce);

            return NoContent();
        }
    }
}
