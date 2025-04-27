using API.RequestHelper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class ProductsController(IGenericRepository<Product> repository) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
            [FromQuery] ProductSpecParams productSpecParams)
        {
            var spec = new ProductSpecification(productSpecParams);

            return await CreatePagesResult(repository, spec, productSpecParams.PageIndex, productSpecParams.PageSize);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repository.GetByIdAsync(id);
            if (product == null) return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repository.Add(product);
            if (!await repository.SaveChangesAsync())
            {
                return StatusCode(500, "An error occurred while creating the product.");
            }
            await repository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id || !ProductExists(id)) return BadRequest("Product not found");
            repository.Update(product);
            if (!await repository.SaveChangesAsync())
            {
                return StatusCode(500, "An error occurred while updating the product.");
            }
            var existingProduct = await repository.GetByIdAsync(id);
            await repository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await repository.GetByIdAsync(id);
            if (product == null) return NotFound("Product not found");

            repository.Remove(product);
            if (!await repository.SaveChangesAsync())
            {
                return StatusCode(500, "An error occurred while deleting the product.");
            }
            await repository.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands()
        {
            // TODO : Implement filtering by brand
            var spec = new BrandListSpecification();
            return Ok(await repository.ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes()
        {
            var spec = new TypeListSpecification();
            return Ok(await repository.ListAsync(spec));
        }

        private bool ProductExists(int id)
        {
            return repository.Exists(id);
        }
    }
}
