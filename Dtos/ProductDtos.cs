using System.ComponentModel.DataAnnotations;

namespace InventoryStockManagement.Dtos;

public record ProductCreateRequest(
    [property: Required, MaxLength(200)] string Name,
    [property: MaxLength(50)] string? ProductCode,
    [property: MaxLength(100)] string? HSNCode,
    bool IsFavourite,
    Guid? CreatedUser,
    List<ProductVariantRequest> Variants);

public record ProductVariantRequest(
    [property: Required, MaxLength(100)] string Name,
    [property: MinLength(1)] List<string> Options);

public record ProductListItemResponse(
    Guid Id,
    string ProductCode,
    string ProductName,
    string? HSNCode,
    decimal TotalStock,
    bool IsFavourite,
    bool Active,
    DateTimeOffset CreatedDate,
    List<ProductVariantResponse> Variants);

public record ProductVariantResponse(string Name, List<string> Options);

public record StockRequest(
    [property: Required] Guid ProductId,
    [property: Range(typeof(decimal), "0.01", "999999999")] decimal Quantity,
    [property: MaxLength(250)] string? Notes);

public record PagedResult<T>(IReadOnlyList<T> Items, int PageNumber, int PageSize, int TotalCount)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
