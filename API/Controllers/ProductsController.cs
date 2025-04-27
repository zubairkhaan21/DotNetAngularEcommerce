using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IGenericRepository<Product> repository) : Controller
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brands, string? types, string? sort)
        {
            var spec = new ProductSpecification(brands, types, sort);
            var products = await repository.ListAsync(spec);

            return Ok(products);
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
