using System.Text.Json.Serialization;

namespace Wex.TransactionManager.Infrastructure.Clients.Models;

public class TreasuryApiResponse
{
    [JsonPropertyName("data")]
    public List<TreasuryRateData> Data { get; set; } = new();
}
