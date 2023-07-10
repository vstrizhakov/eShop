using eShop.Catalog.Entities;
using eShop.Catalog.Repositories;
using eShop.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.Controllers
{
    [Route("api/catalog/categories")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repository;

        public CategoriesController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var ownerId = User.GetAccountId();
            var categories = await _repository.GetCategoriesAsync(ownerId.Value);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory([FromRoute] Guid id)
        {
            var category = await _repository.GetCategoryByIdAsync(id);

            var ownerId = User.GetAccountId();
            if (category == null || category.OwnerId != ownerId)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory([FromBody] Category category)
        {
            var ownerId = User.GetAccountId();
            category.OwnerId = ownerId.Value;

            await _repository.CreateCategoryAsync(category);

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await _repository.GetCategoryByIdAsync(id);

            var ownerId = User.GetAccountId();
            if (category == null || category.OwnerId != ownerId)
            {
                return NotFound();
            }

            await _repository.DeleteCategoryAsync(category);

            return NoContent();
        }
    }
}
