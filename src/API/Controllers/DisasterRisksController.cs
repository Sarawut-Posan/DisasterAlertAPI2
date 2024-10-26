using Application.Interfaces;
using Domain.DTO;
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

        public DisasterRisksController(IRegionRepository regionRepository, IRiskCalculationService riskCalculationService)
        {
            _regionRepository = regionRepository;
            _riskCalculationService = riskCalculationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisasterRiskResponse>>> GetDisasterRisks()
        {
            var regions = await _regionRepository.GetAllAsync();
            var risks = new List<DisasterRiskResponse>();

            foreach (var region in regions)
            {
                foreach (var disasterType in region.DisasterTypes)
                {
                    var risk = await _riskCalculationService.CalculateRiskAsync(region, disasterType);
                    var response = new DisasterRiskResponse
                    {
                        RegionId = risk.RegionId,
                        DisasterType = risk.DisasterType,
                        RiskScore = risk.RiskScore,
                        RiskLevel = risk.RiskLevel,
                        AlertTriggered = risk.AlertTriggered
                    };
                    risks.Add(response);
                }
            }

            return Ok(risks);
        }
    }
}


