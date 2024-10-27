using Application.Interfaces;
using Domain.DTO;
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
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlertSetting>>> GetAllAlertSettings()
        {
            var alertSettings = await _alertSettingRepository.GetAllAsync();
            return Ok(alertSettings);
        }
        
        [HttpGet("{regionId}")]
        public async Task<ActionResult<IEnumerable<AlertSetting>>> GetAlertSettingsForRegion(string regionId)
        {
            var alertSettings = await _alertSettingRepository.GetAllForRegionAsync(regionId);
            return Ok(alertSettings);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddAlertSetting([FromBody] AlertSettingDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var alertSetting = new AlertSetting
            {
                RegionId = dto.RegionID,
                DisasterType = dto.DisasterType,
                ThresholdScore = dto.ThresholdScore
            };

            await _alertSettingRepository.AddAsync(alertSetting);
            return CreatedAtAction(nameof(GetAlertSetting), 
                new { regionId = alertSetting.RegionId, disasterType = alertSetting.DisasterType }, 
                alertSetting);
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

