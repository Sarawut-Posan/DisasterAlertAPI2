using Domain.Entities;

namespace Application.Interfaces;

public interface IAlertSettingRepository
{
    Task<IEnumerable<AlertSetting>> GetAllAsync();
    Task<AlertSetting?> GetByRegionAndDisasterTypeAsync(string regionId, string disasterType);
    Task<IEnumerable<AlertSetting>> GetAllForRegionAsync(string regionId);
    Task AddAsync(AlertSetting alertSetting);
    Task UpdateAsync(AlertSetting alertSetting);
    Task DeleteAsync(string regionId, string disasterType);
}