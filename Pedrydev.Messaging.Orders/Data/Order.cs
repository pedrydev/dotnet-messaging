using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pedrydev.Messaging.Orders.Data;

public sealed class Order
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Code { get; set; }

    public DateTimeOffset Created { get; set; }

    public List<OrderItem> Items { get; set; }
}
