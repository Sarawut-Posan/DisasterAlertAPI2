using System.Net.Http.Json;
using System.Text.Json;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Services.Helper;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class ExternalDataService : IExternalDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ExternalDataService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<WeatherData> GetWeatherDataAsync(double latitude, double longitude)
        {
            var apiKey = _configuration["WeatherApi:Key"];
            var response = await _httpClient.GetFromJsonAsync<WeatherApiResponse>(
                $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric");

            if (response == null)
            {
                throw new Exception("Failed to fetch weather data");
            }

            return new WeatherData
            {
                Temperature = response.Main.Temp,
                Humidity = response.Main.Humidity,
                Rainfall = response.Rain?.OneHour ?? 0
            };
        }

        public async Task<SeismicData> GetSeismicDataAsync(double latitude, double longitude)
        {
            var response = await _httpClient.GetFromJsonAsync<SeismicApiResponse>(
                $"https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&latitude={latitude}&longitude={longitude}&maxradiuskm=100&limit=1");

            if (response?.Features == null || response.Features.Length == 0)
            {
                throw new Exception("Failed to fetch seismic data");
            }

            var feature = response.Features[0];
            return new SeismicData
            {
                Magnitude = feature.Properties.Mag,
                Depth = feature.Geometry.Coordinates[2]
            };
        }
    }
}

