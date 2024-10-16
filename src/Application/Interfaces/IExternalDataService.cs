using Domain.Entities;
namespace Application.Interfaces;

public interface IExternalDataService
{
    Task<WeatherData> GetWeatherDataAsync(double latitude, double longitude);
    Task<SeismicData> GetSeismicDataAsync(double latitude, double longitude);
}