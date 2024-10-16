using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class RiskCalculationService : IRiskCalculationService
    {
        private readonly IExternalDataService _externalDataService;
        private readonly ICacheService _cacheService;
        private readonly ILogger<RiskCalculationService> _logger;

        public RiskCalculationService(
            IExternalDataService externalDataService,
            ICacheService cacheService,
            ILogger<RiskCalculationService> logger)
        {
            _externalDataService = externalDataService;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<DisasterRisk> CalculateRiskAsync(Region region, string disasterType)
        {
            var cacheKey = $"risk_{region.Id}_{disasterType}";
            var cachedRisk = await _cacheService.GetAsync<DisasterRisk>(cacheKey);
            if (cachedRisk != null)
            {
                return cachedRisk;
            }

            try
            {
                var weatherData = await _externalDataService.GetWeatherDataAsync(region.Latitude, region.Longitude);
                var seismicData = await _externalDataService.GetSeismicDataAsync(region.Latitude, region.Longitude);

                var riskScore = CalculateRiskScore(disasterType, weatherData, seismicData);
                var riskLevel = DetermineRiskLevel(riskScore);

                var disasterRisk = new DisasterRisk
                {
                    RegionId = region.Id,
                    DisasterType = disasterType,
                    RiskScore = riskScore,
                    RiskLevel = riskLevel.ToString(),
                    AlertTriggered = riskLevel >= RiskLevel.High
                };

                await _cacheService.SetAsync(cacheKey, disasterRisk, TimeSpan.FromMinutes(15));

                return disasterRisk;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calculating risk for region {region.Id}");
                return new DisasterRisk
                {
                    RegionId = region.Id,
                    DisasterType = disasterType,
                    RiskLevel = RiskLevel.Unknown.ToString(),
                    AlertTriggered = false
                };
            }
        }

        private int CalculateRiskScore(string disasterType, WeatherData weatherData, SeismicData seismicData)
        {
            return disasterType switch
            {
                "flood" => (int)(weatherData.Rainfall * 2),
                "earthquake" => (int)(seismicData.Magnitude * 20),
                "wildfire" => (int)((weatherData.Temperature - weatherData.Humidity) * 2),
                _ => 0,
            };
        }

        private RiskLevel DetermineRiskLevel(int riskScore)
        {
            if (riskScore < 30) return RiskLevel.Low;
            if (riskScore < 70) return RiskLevel.Medium;
            return RiskLevel.High;
        }
    }
}

