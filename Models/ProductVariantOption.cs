using System.ComponentModel.DataAnnotations;

namespace InventoryStockManagement.Models;

public class ProductVariantOption
{
    public Guid Id { get; set; }

    public Guid ProductVariantId { get; set; }

    public ProductVariant ProductVariant { get; set; } = default!;

    [Required]
    [MaxLength(100)]
    public string Value { get; set; } = string.Empty;
}
