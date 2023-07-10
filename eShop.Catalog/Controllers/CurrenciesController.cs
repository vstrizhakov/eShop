using eShop.Catalog.Entities;
using eShop.Catalog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.Controllers
{
    [Route("api/catalog/currencies")]
    [ApiController]
    [Authorize]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyRepository _repository;

        public CurrenciesController(ICurrencyRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Currency>>> GetCurrencies()
        {
            var currencies = await _repository.GetCurrenciesAsync();
            return Ok(currencies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Currency>> GetCurrency([FromRoute] Guid id)
        {
            var currency = await _repository.GetCurrencyByIdAsync(id);

            if (currency == null)
            {
                return NotFound();
            }

            return currency;
        }

        [HttpPost]
        public async Task<ActionResult<Currency>> PostCurrency([FromBody] Currency currency)
        {
            await _repository.CreateCurrencyAsync(currency);

            return CreatedAtAction("GetCurrency", new { id = currency.Id }, currency);
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
