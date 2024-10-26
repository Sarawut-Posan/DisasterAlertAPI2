using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AlertRepository : IAlertRepository
    {
        private readonly ApplicationDbContext _context;

        public AlertRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Alert?> GetByIdAsync(int id)
        {
            return await _context.Alerts.FindAsync(id);
        }

        public async Task<IEnumerable<Alert>> GetAllAsync()
        {
            return await _context.Alerts.ToListAsync();
        }

        public async Task<IEnumerable<Alert>> GetRecentForRegionAsync(string regionId, int count)
        {
            return await _context.Alerts
                .Where(a => a.RegionId == regionId)
                .OrderByDescending(a => a.Timestamp)
                .Take(count)
                .ToListAsync();
        }
        
        

        public async Task AddAsync(Alert alert)
        {
            await _context.Alerts.AddAsync(alert);
            await _context.SaveChangesAsync();
        }

        // public async Task UpdateAsync(Alert alert)
        // {
        //     _context.Alerts.Update(alert);
        //     await _context.SaveChangesAsync();
        // }
        //
        // public async Task DeleteAsync(int id)
        // {
        //     var alert = await _context.Alerts.FindAsync(id);
        //     if (alert != null)
        //     {
        //         _context.Alerts.Remove(alert);
        //         await _context.SaveChangesAsync();
        //     }
        // }
    }
}