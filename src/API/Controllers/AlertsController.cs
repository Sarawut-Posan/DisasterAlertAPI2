using Application.Interfaces;
using Application.Services;
using Domain.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AlertsController : ControllerBase
{
    private readonly IAlertService _alertService;
    private readonly IRiskCalculationService _riskCalculationService;
    private readonly IRegionRepository _regionRepository;
    private readonly IAlertRepository _alertRepository;
    private readonly ILogger<AlertsController> _logger;
    private readonly ICacheService _cacheService;

    public AlertsController(
        IAlertService alertService,
        IRiskCalculationService riskCalculationService,
        IRegionRepository regionRepository,
        ILogger<AlertsController> logger, 
        IAlertRepository alertRepository, 
        ICacheService cacheService)
    {
        _alertService = alertService;
        _riskCalculationService = riskCalculationService;
        _regionRepository = regionRepository;
        _logger = logger;
        _alertRepository = alertRepository;
        _cacheService = cacheService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendAlert([FromBody] SendAlertRequest request)
    {
        try
        {
            var region = await _regionRepository.GetByRegionIDAsync(request.RegionId);
            if (region == null)
            {
                return NotFound($"Region {request.RegionId} not found");
            }

            var alerts = new List<Alert>();
            foreach (var disasterType in region.DisasterTypes)
            {
                var risk = await _riskCalculationService.CalculateRiskAsync(region, disasterType);
                
                if (!risk.AlertTriggered)
                {
                    var alertMessage = _alertService.CreateAlertMessage(risk);
                    var alert = new Alert
                    {
                        RegionId = risk.RegionId,
                        DisasterType = risk.DisasterType,
                        RiskLevel = (RiskLevel)Enum.Parse(typeof(RiskLevel), risk.RiskLevel),
                        AlertMessage = alertMessage,
                        Timestamp = DateTime.UtcNow
                    };
                    alerts.Add(alert);

                    // อัพเดท AlertTriggered เป็น true หลังจากสร้าง alert
                    risk.AlertTriggered = true;
                    
                    // บันทึกการเปลี่ยนแปลงลงใน cache
                    var cacheKey = $"risk_{risk.RegionId}_{risk.DisasterType}";
                    await _cacheService.SetAsync(cacheKey, risk, TimeSpan.FromMinutes(15));
                }
            }

            if (!alerts.Any())
            {
                return Ok(new { message = "No high-risk alerts to send" });
            }

            var results = new List<object>();
            foreach (var alert in alerts)
            {
                var result = await _alertService.SendAlertAsync(alert);
                results.Add(new
                {
                    alertId = result.Id,
                    regionId = result.RegionId,
                    disasterType = result.DisasterType,
                    riskLevel = result.RiskLevel,
                    timestamp = result.Timestamp
                });
            }

            return Ok(new
            {
                message = $"Successfully sent {alerts.Count} alerts via Line Notify",
                alerts = results
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send alert");
            throw;
        }
    }
    
    [HttpGet("GetRecentForRegionAsync")]
    public async Task<IActionResult> GetRecentForRegionAsync(string regionId, int count)
    {
        var alerts = await _alertRepository.GetRecentForRegionAsync(regionId, count);
        return Ok(alerts);
    }
}
