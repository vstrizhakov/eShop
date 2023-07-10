using AutoMapper;
using eShop.Catalog.Entities;
using eShop.Catalog.Models.Products;
using eShop.Catalog.Repositories;
using eShop.Catalog.Services;
using eShop.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.Controllers
{
    [Route("api/catalog/products")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var ownerId = User.GetAccountId();
            var products = await _repository.GetProductsAsync(ownerId.Value);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct([FromRoute] Guid id)
        {
            var product = await _repository.GetProductByIdAsync(id);

            var ownerId = User.GetAccountId();
            if (product == null || product.OwnerId != ownerId)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(
            [FromForm] CreateProductRequest request,
            [FromServices] IFileManager fileManager)
        {
            var product = _mapper.Map<Product>(request);
            var ownerId = User.GetAccountId();
            product.OwnerId = ownerId.Value;

            foreach (var imageFile in request.Images)
            {
                using var imageStream = imageFile.OpenReadStream();
                var imagePath = await fileManager.SaveAsync(Path.Combine("Products", product.Id.ToString(), "Images"), Path.GetExtension(imageFile.FileName), imageStream);
                var productImage = new ProductImage
                {
                    Path = imagePath,
                };

                product.Images.Add(productImage);
            }

            await _repository.CreateProductAsync(product);

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            var product = await _repository.GetProductByIdAsync(id);

            var ownerId = User.GetAccountId();
            if (product == null || product.OwnerId != ownerId)
            {
                return NotFound();
            }

            await _repository.DeleteProductAsync(product);

            return NoContent();
        }
    }
}
