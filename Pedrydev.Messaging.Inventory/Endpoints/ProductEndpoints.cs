using Microsoft.EntityFrameworkCore;
using Pedrydev.Messaging.Inventory.Contracts.Input;
using Pedrydev.Messaging.Inventory.Contracts.Output;
using Pedrydev.Messaging.Inventory.Data;

namespace Pedrydev.Messaging.Inventory.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("product", MapGet)
            .WithOpenApi();
        app.MapPost("product", MapPost)
            .WithOpenApi();
    }

    private static async Task<IResult> MapGet(AppDbContext db, CancellationToken cancellationToken)
    {
        var products = await db.Products.AsNoTracking()
            .Include(static x => x.Category)
            .Select(static x => new ProductModel(
                x.Id, x.Name, x.Price, x.Stock, x.Category.Id, x.Category.Name))
            .ToListAsync(cancellationToken);
        return TypedResults.Ok(products);
    }

    private static async Task<IResult> MapPost(
        CreateProductModel model,
        AppDbContext db,
        CancellationToken cancellationToken)
    {
        var category = await db.Categories.FirstAsync(x => x.Id == model.Category, cancellationToken);
        await db.Products.AddAsync(new()
        {
            Name = model.Name,
            Price = model.Price,
            Stock = model.Stock,
            Category = category
        }, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return Results.Empty;
    }
}
