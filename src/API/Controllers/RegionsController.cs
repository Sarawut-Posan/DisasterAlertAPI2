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

        [HttpGet("{regionID}")]
        public async Task<ActionResult<Region>> GetRegion(string regionID)
        {
            var region = await _regionRepository.GetByRegionIDAsync(regionID);
            if (region == null)
            {
                return NotFound();
            }
            return region;
        }

        [HttpPost]
        public async Task<ActionResult<Region>> CreateRegion(Region region)
        {
            await _regionRepository.AddAsync(region);
            return CreatedAtAction(nameof(GetRegion), new { regionID = region.RegionID }, region);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Region>>> GetAllRegions()
        {
            return Ok(await _regionRepository.GetAllAsync());
        }
    }
}

