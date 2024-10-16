using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertsController : ControllerBase
    {
        private readonly IAlertService _alertService;
        private readonly IAlertRepository _alertRepository;

        public AlertsController(IAlertService alertService, IAlertRepository alertRepository)
        {
            _alertService = alertService;
            _alertRepository = alertRepository;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendAlert([FromBody] Alert alert)
        {
            await _alertService.SendAlertAsync(alert);
            return Ok();
        }

        [HttpGet("{regionId}")]
        public async Task<ActionResult<IEnumerable<Alert>>> GetRecentAlerts(string regionId, [FromQuery] int count = 10)
        {
            var alerts = await _alertRepository.GetRecentForRegionAsync(regionId, count);
            return Ok(alerts);
        }
    }
}
