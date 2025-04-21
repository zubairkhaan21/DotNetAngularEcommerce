using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IProductRepository repository) : Controller
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brands, string? types, string? sort)
        {
            return Ok(await repository.GetProductsAsync(brands, types, sort));
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repository.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return product;
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repository.AddProduct(product);
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
            repository.UpdateProduct(product);
            if (!await repository.SaveChangesAsync())
            {
                return StatusCode(500, "An error occurred while updating the product.");
            }
            var existingProduct = await repository.GetProductByIdAsync(id);
            await repository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await repository.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            repository.DeleteProduct(id);
            if (!await repository.SaveChangesAsync())
            {
                return StatusCode(500, "An error occurred while deleting the product.");
            }
            return NoContent();
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands()
        {
            var brands = await repository.GetProductBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes()
        {
            var types = await repository.GetProductTypesAsync();
            return Ok(types);
        }

        private bool ProductExists(int id)
        {
            return repository.ProductExists(id);
        }
    }
}
