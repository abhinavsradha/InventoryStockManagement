using System.ComponentModel.DataAnnotations;

namespace InventoryStockManagement.Models;

public class Product
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string ProductCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ProductName { get; set; } = string.Empty;

    public DateTimeOffset CreatedDate { get; set; }

    public DateTimeOffset UpdatedDate { get; set; }

    public Guid CreatedUser { get; set; }

    public bool IsFavourite { get; set; }

    public bool Active { get; set; } = true;

    [MaxLength(100)]
    public string? HSNCode { get; set; }

    public decimal TotalStock { get; set; }

    public List<ProductVariant> Variants { get; set; } = [];

    public List<StockTransaction> StockTransactions { get; set; } = [];
}
