using System.Net.Http.Json;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Services.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class ExternalDataService : IExternalDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ExternalDataService> _logger;

        public ExternalDataService(HttpClient httpClient, IConfiguration configuration, ILogger<ExternalDataService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<WeatherData> GetWeatherDataAsync(double latitude, double longitude)
        {
            var apiKey = _configuration["WeatherApi:Key"];
            try
            {
                var response = await _httpClient.GetFromJsonAsync<WeatherApiResponse>(
                    $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric");

                if (response?.Main == null)
                {
                    _logger.LogWarning("Weather data response is null or missing main data for coordinates: {Latitude}, {Longitude}", latitude, longitude);
                    return GetDefaultWeatherData();
                }

                return new WeatherData
                {
                    Temperature = response.Main.Temp,
                    Humidity = response.Main.Humidity,
                    Rainfall = response.Rain?.OneHour ?? 0
                };
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch weather data for coordinates: {Latitude}, {Longitude}", latitude, longitude);
                return GetDefaultWeatherData();
            }
        }

        public async Task<SeismicData> GetSeismicDataAsync(double latitude, double longitude)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<SeismicApiResponse>(
                    $"https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&latitude={latitude}&longitude={longitude}&maxradiuskm=100&limit=1");

                if (response?.Features == null || response.Features.Length == 0)
                {
                    _logger.LogWarning("No seismic data available for coordinates: {Latitude}, {Longitude}. Using default values.", latitude, longitude);
                    return GetDefaultSeismicData();
                }

                var feature = response.Features[0];
                return new SeismicData
                {
                    Magnitude = feature.Properties.Mag,
                    Depth = feature.Geometry.Coordinates[2]
                };
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch seismic data from USGS API for coordinates: {Latitude}, {Longitude}", latitude, longitude);
                return GetDefaultSeismicData();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching seismic data for coordinates: {Latitude}, {Longitude}", latitude, longitude);
                return GetDefaultSeismicData();
            }
        }

        private WeatherData GetDefaultWeatherData()
        {
            return new WeatherData
            {
                Temperature = 0.0,
                Humidity = 0,
                Rainfall = 0
            };
        }

        private SeismicData GetDefaultSeismicData()
        {
            return new SeismicData
            {
                Magnitude = 0.0f, 
                Depth = 0.0f
            };
        }
    }
}
