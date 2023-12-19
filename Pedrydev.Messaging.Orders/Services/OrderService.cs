using MassTransit;
using MongoDB.Driver;
using Pedrydev.Messaging.Orders.Contracts;
using Pedrydev.Messaging.Orders.Data;
using Pedrydev.Messaging.Orders.Models.Input;

namespace Pedrydev.Messaging.Orders.Services;

public sealed class OrderService
{
    private readonly IMongoCollection<Order> _collection;
    private readonly InventoryService _inventoryService;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public OrderService(IMongoDatabase database, InventoryService inventoryService, ISendEndpointProvider sendEndpointProvider)
    {
        _inventoryService = inventoryService;
        _sendEndpointProvider = sendEndpointProvider;
        _collection = database.GetCollection<Order>("orders");
    }

    public async Task Create(CreateOrderModel model, CancellationToken cancellationToken)
    {
        var products = await _inventoryService.GetProducts(cancellationToken);
        var order = new Order
        {
            Created = DateTimeOffset.UtcNow,
            Items = model.OrderItems.Select(x => new OrderItem
            {
                Product = x.Product,
                Price = products.First(xx => x.Product == xx.Id).Price,
                Quantity = x.Quantity
            }).ToList()
        };
        await _collection.InsertOneAsync(order, new InsertOneOptions(), cancellationToken);
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new("queue:order_created"));
        await endpoint.Send(new OrderCreated
        {
            Products = model.OrderItems.Select(static x => new OrderProduct
            {
                Id = x.Product,
                Quantity = x.Quantity
            }).ToList()
        }, cancellationToken);
    }

    public async Task<IEnumerable<Order>> Get(CancellationToken cancellationToken)
        => await _collection
            .Find(static x => true, options: null)
            .ToListAsync(cancellationToken);
}
