using Domain.Entities;

namespace Application.Interfaces;

public interface IRiskCalculationService
{
    Task<DisasterRisk> CalculateRiskAsync(Region region, string disasterType);
}