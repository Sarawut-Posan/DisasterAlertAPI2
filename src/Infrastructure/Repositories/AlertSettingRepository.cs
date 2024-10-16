using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AlertSettingRepository : IAlertSettingRepository
    {
        private readonly ApplicationDbContext _context;

        public AlertSettingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AlertSetting>> GetAllAsync()
        {
            return await _context.AlertSettings.ToListAsync();
        }

        public async Task<AlertSetting?> GetByRegionAndDisasterTypeAsync(string regionId, string disasterType)
        {
            return await _context.AlertSettings.FindAsync(regionId, disasterType);
        }

        public async Task<IEnumerable<AlertSetting>> GetAllForRegionAsync(string regionId)
        {
            return await _context.AlertSettings.Where(a => a.RegionId == regionId).ToListAsync();
        }
        public async Task AddAsync(AlertSetting alertSetting)
        {
            await _context.AlertSettings.AddAsync(alertSetting);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AlertSetting alertSetting)
        {
            _context.AlertSettings.Update(alertSetting);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string regionId, string disasterType)
        {
            var alertSetting = await _context.AlertSettings.FindAsync(regionId, disasterType);
            if (alertSetting != null)
            {
                _context.AlertSettings.Remove(alertSetting);
                await _context.SaveChangesAsync();
            }
        }
    }
}