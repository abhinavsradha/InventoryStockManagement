using System.ComponentModel.DataAnnotations;

namespace InventoryStockManagement.Models;

public class StockTransaction
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Product Product { get; set; } = default!;

    public StockTransactionType TransactionType { get; set; }

    public decimal Quantity { get; set; }

    [MaxLength(250)]
    public string? Notes { get; set; }

    public DateTimeOffset TransactionDate { get; set; }
}

public enum StockTransactionType
{
    Purchase = 1,
    Sale = 2
}
