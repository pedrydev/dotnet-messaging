namespace Pedrydev.Messaging.Orders.Contracts;

public sealed class OrderCreated
{
    public List<OrderProduct> Products { get; set; }
}

public sealed class OrderProduct
{
    public int Id { get; set; }

    public int Quantity { get; set; }
}
