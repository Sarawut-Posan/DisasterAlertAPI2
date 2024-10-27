using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisasterRisksController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IRiskCalculationService _riskCalculationService;
        private readonly ILogger<DisasterRisksController> _logger;

        public DisasterRisksController(
            IRegionRepository regionRepository,
            IRiskCalculationService riskCalculationService,
            ILogger<DisasterRisksController> logger)
        {
            _regionRepository = regionRepository;
            _riskCalculationService = riskCalculationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisasterRisk>>> GetDisasterRisks()
        {
            try
            {
                var regions = await _regionRepository.GetAllAsync();
                var risks = new List<DisasterRisk>();

                foreach (var region in regions)
                {
                    foreach (var disasterType in region.DisasterTypes)
                    {
                        var risk = await _riskCalculationService.CalculateRiskAsync(region, disasterType);
                        risks.Add(new DisasterRisk
                        {
                            RegionId = region.RegionID,
                            DisasterType = disasterType,
                            RiskScore = risk.RiskScore,
                            RiskLevel = risk.RiskLevel,
                            AlertTriggered = risk.AlertTriggered
                        });
                    }
                }

                return Ok(risks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting disaster risks");
                throw;
            }
        }
    }
}


