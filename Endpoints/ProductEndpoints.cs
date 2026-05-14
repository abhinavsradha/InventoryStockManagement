using InventoryStockManagement.Dtos;
using InventoryStockManagement.Services;

namespace InventoryStockManagement.Endpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/products")
            .WithTags("Products");

        group.MapPost("/", async (ProductCreateRequest request, IProductService service, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ProductEndpoints");

            try
            {
                var product = await service.CreateAsync(request);
                return Results.Created($"/api/products/{product.Id}", product);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Product creation failed");
                return Results.Problem("Unable to create product.");
            }
        });

        group.MapGet("/", async (IProductService service, int pageNumber = 1, int pageSize = 10, string? search = null) =>
        {
            var result = await service.ListAsync(pageNumber, pageSize, search);
            return Results.Ok(result);
        });

        group.MapPost("/stock/purchase", async (StockRequest request, IProductService service) =>
        {
            try
            {
                await service.AddStockAsync(request);
                return Results.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        });

        group.MapPost("/stock/sale", async (StockRequest request, IProductService service) =>
        {
            try
            {
                await service.RemoveStockAsync(request);
                return Results.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { message = ex.Message });
            }
        });

        return group;
    }
}
