using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository(StoreContext context) : IProductRepository
{
    public void AddProduct(Product product)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));
        context.Products.Add(product);
        // context.SaveChanges(); // This should be handled in the SaveChangesAsync method
    }

    public void DeleteProduct(int id)
    {
        var product = context.Products.Find(id);
        if (product == null) throw new ArgumentException($"Product with ID {id} not found.");
        context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brands, string? types, string? sort)
    {
        var query = context.Products.AsQueryable();
        if (!string.IsNullOrWhiteSpace(brands))
        query = query.Where(p => p.Brand == brands);
        
        if (!string.IsNullOrWhiteSpace(types))
        query = query.Where(p => p.Type == types);
        query = sort switch
            {
                "priceAsc" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetProductBrandsAsync()
    {
        return await context.Products.Select(p => p.Brand).Distinct().ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<string>> GetProductTypesAsync()
    {
        return await context.Products.Select(p => p.Type).Distinct().ToListAsync();
    }

    public bool ProductExists(int id)
    {
        return context.Products.Any(p => p.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        try
        {
            return await context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateConcurrencyException)
        {
            // Handle concurrency exception if needed
            return false;
        }
        catch (DbUpdateException ex)
        {
            // Handle database update exception if needed
            Console.WriteLine($"Database update error: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            // Handle other exceptions if needed
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
    }

    public void UpdateProduct(Product product)
    {
        context.Entry(product).State = EntityState.Modified;
    }
}
