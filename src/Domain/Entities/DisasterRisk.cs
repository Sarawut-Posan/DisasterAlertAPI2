namespace Domain.Entities;

public class DisasterRisk
{
    public int RegionId { get; set; }
    public string DisasterType { get; set; }
    public double RiskScore { get; set; }
    public string RiskLevel { get; set; }
    public bool AlertTriggered { get; set; }
}
public enum RiskLevel
{
    Unknown,
    Low,
    Medium,
    High
}