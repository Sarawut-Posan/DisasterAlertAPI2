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

        public DisasterRisksController(IRegionRepository regionRepository, IRiskCalculationService riskCalculationService)
        {
            _regionRepository = regionRepository;
            _riskCalculationService = riskCalculationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisasterRisk>>> GetDisasterRisks()
        {
            var regions = await _regionRepository.GetAllAsync();
            var risks = new List<DisasterRisk>();

            foreach (var region in regions)
            {
                foreach (var disasterType in region.DisasterTypes)
                {
                    var risk = await _riskCalculationService.CalculateRiskAsync(region, disasterType);
                    risks.Add(risk);
                }
            }

            return Ok(risks);
        }
    }
}


