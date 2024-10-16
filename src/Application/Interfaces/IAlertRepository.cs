using Domain.Entities;

namespace Application.Interfaces;

public interface IAlertRepository
{
    Task<IEnumerable<Alert>> GetRecentForRegionAsync(string regionId, int count);
    Task AddAsync(Alert alert);
}