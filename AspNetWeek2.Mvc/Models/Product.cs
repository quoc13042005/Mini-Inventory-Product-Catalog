using System.ComponentModel.DataAnnotations;

namespace AspNetWeek2.Mvc.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
