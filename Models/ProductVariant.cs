using System.ComponentModel.DataAnnotations;

namespace InventoryStockManagement.Models;

public class ProductVariant
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Product Product { get; set; } = default!;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public List<ProductVariantOption> Options { get; set; } = [];
}
