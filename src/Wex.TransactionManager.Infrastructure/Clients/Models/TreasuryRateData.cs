using System.Text.Json.Serialization;

namespace Wex.TransactionManager.Infrastructure.Clients.Models;

public class TreasuryRateData
{
    [JsonPropertyName("country_currency_desc")]
    public string CountryCurrencyDesc { get; set; } = string.Empty;
    
    [JsonPropertyName("exchange_rate")]
    public string ExchangeRate { get; set; } = string.Empty;
    
    [JsonPropertyName("record_date")]
    public string RecordDate { get; set; } = string.Empty;
}
