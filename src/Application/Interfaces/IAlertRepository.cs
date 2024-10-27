using Domain.Entities;

namespace Application.Interfaces;

public interface IAlertRepository
{
    Task<Alert> AddAsync(Alert alert);  
    Task<IEnumerable<Alert>> GetRecentForRegionAsync(string regionId, int count);
}
