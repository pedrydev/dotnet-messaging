using System.Text.Json;

namespace Pedrydev.Messaging.Orders;

public static class GlobalOptions
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new (JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };
}
