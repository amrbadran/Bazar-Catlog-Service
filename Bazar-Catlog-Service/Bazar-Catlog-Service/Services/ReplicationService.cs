using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bazar_Catlog_Service.Services;

public class ReplicationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ReplicationService> _logger;
    private readonly List<string> _replicaUrls;

    public ReplicationService(HttpClient httpClient, ILogger<ReplicationService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        var replicaList = configuration["REPLICA_URLS"] ?? "";
        _replicaUrls = replicaList.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Where(url => !string.IsNullOrWhiteSpace(url))
            .ToList();
    }

    public async Task PropagateWriteAsync(string endpoint, object data)
    {
        var tasks = _replicaUrls.Select(url => PropagateToReplicaAsync(url, endpoint, data));
        await Task.WhenAll(tasks);
    }

    private async Task PropagateToReplicaAsync(string replicaUrl, string endpoint, object data)
    {
        try
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var fullUrl = $"{replicaUrl.Trim()}{endpoint}";
            
            var request = new HttpRequestMessage(HttpMethod.Patch, fullUrl)
            {
                Content = content
            };
            request.Headers.Add("X-Replication", "true");
            
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Failed to replicate to {fullUrl}: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error replicating to {replicaUrl}");
        }
    }
}

