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
    private readonly ILogger<AlertsController> _logger;
    private readonly IAlertRepository _alertRepository;

    public AlertsController(
        IAlertService alertService,
        ILogger<AlertsController> logger
         )
    {
        _alertService = alertService;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendAlert([FromBody] CreateAlertRequest request)
    {
        try
        {
            var alert = new Alert
            {
                RegionId = request.RegionId,
                DisasterType = request.DisasterType,
                RiskLevel = request.RiskLevel,
                AlertMessage = request.Message,
                Timestamp = DateTime.UtcNow
            };

            // ไม่จำเป็นต้องมี RecipientPhoneNumbers แล้ว เพราะใช้ Line Notify
            var result = await _alertService.SendAlertAsync(alert);

            return Ok(new
            {
                message = "Alert sent successfully via Line Notify",
                alertId = result.Id,
                timestamp = result.Timestamp
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send alert");
            throw;
        }
    }

    [HttpGet("{regionId}")]
    public async Task<ActionResult<IEnumerable<Alert>>> GetRecentAlerts(
        string regionId,
        [FromQuery] int count = 10)
    {
        var alerts = await _alertRepository.GetRecentForRegionAsync(regionId, count);
        return Ok(alerts);
    }
}
