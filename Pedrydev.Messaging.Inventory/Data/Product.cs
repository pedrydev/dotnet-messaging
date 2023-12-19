namespace Pedrydev.Messaging.Inventory.Data;

public sealed class Product
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required decimal Price { get; set; }

    public required int Stock { get; set; }

    public required Category Category { get; set; }
}
