using System.Text.Json;
using Pedrydev.Messaging.Inventory.Contracts.Output;

namespace Pedrydev.Messaging.Orders.Services;

public sealed class InventoryService
{
    private readonly HttpClient _httpClient;

    public InventoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ProductModel>> GetProducts(CancellationToken cancellationToken)
    {
        await using var stream = await _httpClient.GetStreamAsync("product", cancellationToken);
        var products = await JsonSerializer.DeserializeAsync<List<ProductModel>>(
            stream,
            options: GlobalOptions.JsonSerializerOptions,
            cancellationToken: cancellationToken);
        return products ?? Enumerable.Empty<ProductModel>();
    }
}
