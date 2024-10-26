namespace Domain.DTO;

public class DisasterRiskResponse
{
    public int RegionId { get; set; }
    public string DisasterType { get; set; }
    public double RiskScore { get; set; }
    public string RiskLevel { get; set; }
    public bool AlertTriggered { get; set; }
}