using InventoryStockManagement.Data;
using InventoryStockManagement.Dtos;
using InventoryStockManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryStockManagement.Services;

public interface IProductService
{
    Task<ProductListItemResponse> CreateAsync(ProductCreateRequest request, CancellationToken cancellationToken = default);

    Task<PagedResult<ProductListItemResponse>> ListAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default);

    Task AddStockAsync(StockRequest request, CancellationToken cancellationToken = default);

    Task RemoveStockAsync(StockRequest request, CancellationToken cancellationToken = default);
}

public class ProductService(InventoryDbContext dbContext, ILogger<ProductService> logger) : IProductService
{
    public async Task<ProductListItemResponse> CreateAsync(ProductCreateRequest request, CancellationToken cancellationToken = default)
    {
        ValidateProduct(request);

        var productCode = string.IsNullOrWhiteSpace(request.ProductCode)
            ? await GenerateProductCodeAsync(cancellationToken)
            : request.ProductCode.Trim();

        var exists = await dbContext.Products.AnyAsync(product => product.ProductCode == productCode, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException($"Product code '{productCode}' already exists.");
        }

        var now = DateTimeOffset.UtcNow;
        var product = new Product
        {
            Id = Guid.NewGuid(),
            ProductCode = productCode,
            ProductName = request.Name.Trim(),
            HSNCode = string.IsNullOrWhiteSpace(request.HSNCode) ? null : request.HSNCode.Trim(),
            CreatedDate = now,
            UpdatedDate = now,
            CreatedUser = request.CreatedUser ?? Guid.Empty,
            IsFavourite = request.IsFavourite,
            Active = true,
            TotalStock = 0
        };

        foreach (var variantRequest in request.Variants)
        {
            product.Variants.Add(new ProductVariant
            {
                Id = Guid.NewGuid(),
                Name = variantRequest.Name.Trim(),
                Options = variantRequest.Options
                    .Where(option => !string.IsNullOrWhiteSpace(option))
                    .Select(option => new ProductVariantOption
                    {
                        Id = Guid.NewGuid(),
                        Value = option.Trim()
                    })
                    .ToList()
            });
        }

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Created product {ProductId} with code {ProductCode}", product.Id, product.ProductCode);
        return ToResponse(product);
    }

    public async Task<PagedResult<ProductListItemResponse>> ListAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Clamp(pageSize, 1, 50);

        var query = dbContext.Products
            .AsNoTracking()
            .Include(product => product.Variants)
            .ThenInclude(variant => variant.Options)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(product =>
                product.ProductName.Contains(term) ||
                product.ProductCode.Contains(term) ||
                (product.HSNCode != null && product.HSNCode.Contains(term)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(product => product.ProductName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(product => ToResponse(product))
            .ToListAsync(cancellationToken);

        return new PagedResult<ProductListItemResponse>(items, pageNumber, pageSize, totalCount);
    }

    public async Task AddStockAsync(StockRequest request, CancellationToken cancellationToken = default)
    {
        await ChangeStockAsync(request, StockTransactionType.Purchase, cancellationToken);
    }

    public async Task RemoveStockAsync(StockRequest request, CancellationToken cancellationToken = default)
    {
        await ChangeStockAsync(request, StockTransactionType.Sale, cancellationToken);
    }

    private async Task ChangeStockAsync(StockRequest request, StockTransactionType transactionType, CancellationToken cancellationToken)
    {
        if (request.Quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.");
        }

        var product = await dbContext.Products.FirstOrDefaultAsync(product => product.Id == request.ProductId, cancellationToken)
            ?? throw new KeyNotFoundException("Product not found.");

        if (transactionType == StockTransactionType.Sale && product.TotalStock < request.Quantity)
        {
            throw new InvalidOperationException("Insufficient stock for this sale.");
        }

        product.TotalStock += transactionType == StockTransactionType.Purchase ? request.Quantity : -request.Quantity;
        product.UpdatedDate = DateTimeOffset.UtcNow;

        dbContext.StockTransactions.Add(new StockTransaction
        {
            Id = Guid.NewGuid(),
            ProductId = product.Id,
            TransactionType = transactionType,
            Quantity = request.Quantity,
            Notes = request.Notes,
            TransactionDate = DateTimeOffset.UtcNow
        });

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("{TransactionType} stock for product {ProductId}. Quantity: {Quantity}", transactionType, product.Id, request.Quantity);
    }

    private async Task<string> GenerateProductCodeAsync(CancellationToken cancellationToken)
    {
        var count = await dbContext.Products.CountAsync(cancellationToken) + 1;
        return $"PRD-{count:0000}";
    }

    private static void ValidateProduct(ProductCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Product name is required.");
        }

        if (request.Variants.Count == 0)
        {
            throw new ArgumentException("At least one variant is required.");
        }

        foreach (var variant in request.Variants)
        {
            if (string.IsNullOrWhiteSpace(variant.Name))
            {
                throw new ArgumentException("Variant name is required.");
            }

            if (variant.Options.Count == 0 || variant.Options.All(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException($"Variant '{variant.Name}' must have at least one option.");
            }
        }
    }

    private static ProductListItemResponse ToResponse(Product product)
    {
        return new ProductListItemResponse(
            product.Id,
            product.ProductCode,
            product.ProductName,
            product.HSNCode,
            product.TotalStock,
            product.IsFavourite,
            product.Active,
            product.CreatedDate,
            product.Variants
                .Select(variant => new ProductVariantResponse(
                    variant.Name,
                    variant.Options.Select(option => option.Value).ToList()))
                .ToList());
    }
}
