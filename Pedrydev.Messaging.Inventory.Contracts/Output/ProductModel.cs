namespace Pedrydev.Messaging.Inventory.Contracts.Output;

public sealed record ProductModel(int Id, string Name, decimal Price, int Stock, int CategoryId, string CategoryName);
