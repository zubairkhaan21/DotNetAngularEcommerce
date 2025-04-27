using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreateProductDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    [Required]
    public string ImageUrl { get; set; } = string.Empty;
    [Required]
    public string Type { get; set; } = string.Empty;
    [Required]
    public string Brand { get; set; } = string.Empty;
    [Range(0, int.MaxValue, ErrorMessage = "Quantity in stock must be greater than or equal to 0")]
    public int QuantityInStock { get; set; }
}
