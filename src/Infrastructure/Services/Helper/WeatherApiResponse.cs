namespace Infrastructure.Services.Helper;

public class WeatherApiResponse
{
    public MainData Main { get; set; }
    public RainData? Rain { get; set; }
}