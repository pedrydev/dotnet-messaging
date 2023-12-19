using MassTransit;
using Microsoft.EntityFrameworkCore;
using Pedrydev.Messaging.Inventory.Data;
using Pedrydev.Messaging.Orders.Contracts;

namespace Pedrydev.Messaging.Inventory.Consumers;

public sealed class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    private readonly AppDbContext _db;
    private readonly ILogger<OrderCreatedConsumer> _logger;

    public OrderCreatedConsumer(AppDbContext db, ILogger<OrderCreatedConsumer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var productIds = context.Message.Products.Select(static x => x.Id);
        var products = await _db.Products
            .AsNoTracking()
            .Where(x => productIds.Contains(x.Id))
            .ToListAsync(context.CancellationToken);
        foreach (var product in products)
            product.Stock -= context.Message.Products.Single(x => x.Id == product.Id).Quantity;
        _db.Products.UpdateRange(products);
        await _db.SaveChangesAsync(context.CancellationToken);
        _logger.LogInformation("Stock updated");
    }
}
