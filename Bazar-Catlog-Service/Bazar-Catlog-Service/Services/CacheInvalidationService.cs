using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bazar_Catlog_Service.Services;

public class CacheInvalidationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CacheInvalidationService> _logger;
    private readonly string _gatewayUrl;

    public CacheInvalidationService(HttpClient httpClient, ILogger<CacheInvalidationService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _gatewayUrl = configuration["GATEWAY_URL"] ?? "http://gateway-service:3000";
    }

    public async Task InvalidateBookCacheAsync(int bookId)
    {
        try
        {
            var payload = new { bookId };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"{_gatewayUrl}/cache/invalidate", content);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Failed to invalidate cache for book {bookId}: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error invalidating cache for book {bookId}");
        }
    }
}

