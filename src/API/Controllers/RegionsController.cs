using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;

        public RegionsController(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddRegion([FromBody] Region region)
        {
            await _regionRepository.AddAsync(region);
            return CreatedAtAction(nameof(GetRegion), new { id = region.Id }, region);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Region>> GetRegion(string id)
        {
            var region = await _regionRepository.GetByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            return region;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Region>>> GetAllRegions()
        {
            return Ok(await _regionRepository.GetAllAsync());
        }
    }
}

