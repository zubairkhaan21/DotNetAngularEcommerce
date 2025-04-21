using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetProductByIdAsync(int id);
    Task<IReadOnlyList<Product>> GetProductsAsync(string? brands, string? types, string? sort);
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(int id);
    bool ProductExists(int id);
    Task<bool> SaveChangesAsync();

    Task<IReadOnlyList<string>> GetProductBrandsAsync();
    Task<IReadOnlyList<string>> GetProductTypesAsync();
}
