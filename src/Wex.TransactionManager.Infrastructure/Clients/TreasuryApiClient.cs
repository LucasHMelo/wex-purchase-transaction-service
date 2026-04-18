using System;
using System.Globalization;
using System.Text.Json;
using Wex.TransactionManager.Application.DTOs;
using Wex.TransactionManager.Application.Interfaces;
using Wex.TransactionManager.Infrastructure.Clients.Models;

namespace Wex.TransactionManager.Infrastructure.Clients;

public class TreasuryApiClient : ITreasuryApiClient
{
    private readonly HttpClient _httpClient;
    
    public TreasuryApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        
        _httpClient.BaseAddress = new Uri("https://api.fiscaldata.treasury.gov");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Wex-Transaction");

    }

    public async Task<decimal?> GetExchangeRateAsync(string currency, DateTime date, CancellationToken cancellationToken = default)
    {
        try
        {
            // Treasury API expects date in YYYY-MM-DD format for daily rates
            var dateParam = date.ToString("YYYY-MM-DD");
            
            // Build the request URL for Treasury API
            var requestUrl = $"/services/api/fiscal_service/v1/accounting/od/rates_of_exchange" +
                           $"?fields=country_currency_desc,exchange_rate,record_date" +
                           $"&filter=record_date:lte:{dateParam},country_currency_desc:eq:{currency}" +
                           $"&sort=-record_date" +
                           $"&page[size]=1";

            var fullUrl = $"{_httpClient.BaseAddress}{requestUrl}";

            var response = await _httpClient.GetAsync(requestUrl, cancellationToken);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var treasuryResponse = JsonSerializer.Deserialize<TreasuryApiResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Parse the exchange rate from string to decimal
            decimal? rate = null;
            var firstRecord = treasuryResponse?.Data?.FirstOrDefault();
            if (firstRecord != null && !string.IsNullOrEmpty(firstRecord.ExchangeRate))
            {
                if (decimal.TryParse(firstRecord.ExchangeRate, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedRate))
                {
                    rate = parsedRate;
                }
            }
            return rate;
        }
        catch (Exception ex)
        {
            
            return null;
        }
    }

    public async Task<List<ExchangeRateDto>> GetExchangeRatesRangeAsync(string currency, DateTime startDate, CancellationToken cancellationToken = default)
    {
        try
        {
            var startDateParam = startDate.ToString("yyyy-MM-dd");
            var requestUrl = $"services/api/fiscal_service/v1/accounting/od/rates_of_exchange" +
                           $"?fields=country_currency_desc,exchange_rate,record_date" +
                           $"&filter=country_currency_desc:eq:{currency},record_date:gte:{startDateParam}" +
                           $"&sort=-record_date" +
                           $"&page[size]=1000"; 

            var response = await _httpClient.GetAsync(requestUrl, cancellationToken);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var treasuryResponse = JsonSerializer.Deserialize<TreasuryApiResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var rates = treasuryResponse?.Data?
                .Where(d => !string.IsNullOrEmpty(d.CountryCurrencyDesc) && !string.IsNullOrEmpty(d.ExchangeRate))
                .Select(d => new ExchangeRateDto
                {
                    Currency = d.CountryCurrencyDesc,
                    Rate = decimal.TryParse(d.ExchangeRate, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedRate) ? parsedRate : 0,
                    Date = DateTime.Parse(d.RecordDate),
                    RecordDate = DateTime.Parse(d.RecordDate)
                })
                .ToList() ?? new List<ExchangeRateDto>();

            return rates;
        }
        catch (Exception ex)
        {
            
            return new List<ExchangeRateDto>();
        }
    }

}
