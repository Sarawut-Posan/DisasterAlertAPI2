using Application.Configuration;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class LineNotifyService : INotificationService
{
    private readonly HttpClient _httpClient;
    private readonly string _accessToken;
    private readonly ILogger<LineNotifyService> _logger;

    public LineNotifyService(
        HttpClient httpClient,
        IOptions<LineNotifySettings> settings,
        ILogger<LineNotifyService> logger)
    {
        _httpClient = httpClient;
        _accessToken = settings.Value.AccessToken;
        _logger = logger;
        
        _httpClient.BaseAddress = new Uri("https://notify-api.line.me/");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
    }

    public async Task SendNotificationAsync(string message)
    {
        try
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("message", message)
            });

            var response = await _httpClient.PostAsync("api/notify", content);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation("Line notification sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send Line notification");
            throw new ExternalApiException("Failed to send Line notification", ex);
        }
    }
}