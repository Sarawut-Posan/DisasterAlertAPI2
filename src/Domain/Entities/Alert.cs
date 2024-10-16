namespace Domain.Entities;

public class Alert
{
    public string Id { get; set; } = string.Empty;
    public string RegionId { get; set; } = string.Empty;
    public string DisasterType { get; set; } = string.Empty;
    public RiskLevel RiskLevel { get; set; }
    public string AlertMessage { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}