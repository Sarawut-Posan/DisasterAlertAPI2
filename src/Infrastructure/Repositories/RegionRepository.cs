using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly ApplicationDbContext _context;

        public RegionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Region?> GetByIdAsync(string id)
        {
            return await _context.Regions.FindAsync(id);
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _context.Regions.ToListAsync();
        }

        public async Task AddAsync(Region region)
        {
            await _context.Regions.AddAsync(region);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Region region)
        {
            _context.Regions.Update(region);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region != null)
            {
                _context.Regions.Remove(region);
                await _context.SaveChangesAsync();
            }
        }
    }
}


