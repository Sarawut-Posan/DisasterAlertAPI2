using Domain.Entities;

namespace Application.Interfaces;

public interface IRegionRepository
{
    Task<Region> GetByRegionIDAsync(string regionID);
    Task<IEnumerable<Region>> GetAllAsync();
    Task AddAsync(Region region);
    Task UpdateAsync(Region region);
    Task DeleteAsync(string id);

}