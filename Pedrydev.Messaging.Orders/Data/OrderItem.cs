using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pedrydev.Messaging.Orders.Data;

public sealed class OrderItem
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public int Product { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }
}
