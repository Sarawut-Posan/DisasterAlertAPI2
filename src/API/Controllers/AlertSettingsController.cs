using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertSettingsController : ControllerBase
    {
        private readonly IAlertSettingRepository _alertSettingRepository;

        public AlertSettingsController(IAlertSettingRepository alertSettingRepository)
        {
            _alertSettingRepository = alertSettingRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddAlertSetting([FromBody] AlertSetting alertSetting)
        {
            await _alertSettingRepository.AddAsync(alertSetting);
            return CreatedAtAction(nameof(GetAlertSetting), new { regionId = alertSetting.RegionId, disasterType = alertSetting.DisasterType }, alertSetting);
        }

        [HttpGet("{regionId}/{disasterType}")]
        public async Task<ActionResult<AlertSetting>> GetAlertSetting(string regionId, string disasterType)
        {
            var alertSetting = await _alertSettingRepository.GetByRegionAndDisasterTypeAsync(regionId, disasterType);
            if (alertSetting == null)
            {
                return NotFound();
            }
            return alertSetting;
        }
    }
}

