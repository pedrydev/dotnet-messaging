using Pedrydev.Messaging.Orders.Models.Input;
using Pedrydev.Messaging.Orders.Services;

namespace Pedrydev.Messaging.Orders.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        app.MapGet("order", MapGet)
            .WithOpenApi();
        app.MapPost("order", MapPost)
            .WithOpenApi();
    }

    private static async Task<IResult> MapGet(
        OrderService service,
        CancellationToken cancellationToken)
    {
        var orders = await service.Get(cancellationToken);
        return TypedResults.Ok(orders);
    }

    private static async Task<IResult> MapPost(
        CreateOrderModel model,
        OrderService service,
        CancellationToken cancellationToken)
    {
        await service.Create(model, cancellationToken);
        return TypedResults.Empty;
    }
}
