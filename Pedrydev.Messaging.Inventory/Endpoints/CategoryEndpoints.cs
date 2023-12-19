using Microsoft.EntityFrameworkCore;
using Pedrydev.Messaging.Inventory.Contracts.Input;
using Pedrydev.Messaging.Inventory.Data;

namespace Pedrydev.Messaging.Inventory.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        app.MapGet("category", MapGet)
            .WithOpenApi();
        app.MapPost("category", MapPost)
            .WithOpenApi();
    }

    private static async Task<IResult> MapGet(AppDbContext db, CancellationToken cancellationToken)
    {
        var categories = await db.Categories.AsNoTracking().ToListAsync(cancellationToken);
        return TypedResults.Ok(categories);
    }

    private static async Task<IResult> MapPost(
        CreateCategoryModel model,
        AppDbContext db,
        CancellationToken cancellationToken)
    {
        await db.Categories.AddAsync(new() { Name = model.Name }, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return Results.Empty;
    }
}
