using Domain.Entities;

namespace Application.Interfaces;

public interface IRegionRepository
{
    Task<Region?> GetByIdAsync(string id);
    Task<IEnumerable<Region>> GetAllAsync();
    Task AddAsync(Region region);
    Task UpdateAsync(Region region);
    Task DeleteAsync(string id);
}