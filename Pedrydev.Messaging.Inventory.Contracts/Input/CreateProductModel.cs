namespace Pedrydev.Messaging.Inventory.Contracts.Input;

public sealed record CreateProductModel(string Name, decimal Price, int Stock, int Category);
