namespace Pedrydev.Messaging.Orders.Models.Input;

public sealed record CreateOrderModel(OrderItemModel[] OrderItems);

public sealed record OrderItemModel(int Product, int Quantity);
