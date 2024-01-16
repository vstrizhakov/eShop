using AutoMapper;
using eShopping.Catalog.Entities;
using eShopping.Catalog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShopping.Catalog.Controllers
{
    [Route("api/catalog/currencies")]
    [ApiController]
    [Authorize]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyRepository _repository;
        private readonly IMapper _mapper;

        public CurrenciesController(ICurrencyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Currencies.Currency>>> GetCurrencies()
        {
            var currencies = await _repository.GetCurrenciesAsync();
            var response = _mapper.Map<IEnumerable<Models.Currencies.Currency>>(currencies);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Currencies.Currency>> GetCurrency([FromRoute] Guid id)
        {
            var currency = await _repository.GetCurrencyByIdAsync(id);

            if (currency == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<Models.Currencies.Currency>(currency);
            return response;
        }

        [HttpPost]
        public async Task<ActionResult<Models.Currencies.Currency>> PostCurrency([FromBody] Models.Currencies.CreateCurrencyRequest request)
        {
            var currency = _mapper.Map<Currency>(request);

            await _repository.CreateCurrencyAsync(currency);

            var response = _mapper.Map<Models.Currencies.Currency>(currency);
            return CreatedAtAction("GetCurrency", new { id = currency.Id }, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurrency([FromRoute] Guid id)
        {
            var currency = await _repository.GetCurrencyByIdAsync(id);
            if (currency == null)
            {
                return NotFound();
            }

            await _repository.DeleteCurrencyAsync(currency);

            return NoContent();
        }
    }
}
