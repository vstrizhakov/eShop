using AutoMapper;
using eShop.Catalog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.Controllers
{
    [Route("api/catalog/shops")]
    [ApiController]
    [Authorize]
    public class ShopsController : ControllerBase
    {
        private readonly IShopService _shopService;
        private readonly IMapper _mapper;

        public ShopsController(IShopService shopService, IMapper mapper)
        {
            _shopService = shopService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Shops.Shop>>> GetShops()
        {
            var shops = await _shopService.GetShopsAsync();
            var response = _mapper.Map<IEnumerable<Models.Shops.Shop>>(shops);
            return Ok(response);
        }
    }
}
