namespace Domain.Entities;

public class Alert
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();  
    public string RegionId { get; set; }
    public string DisasterType { get; set; }
    public RiskLevel RiskLevel { get; set; }
    public string AlertMessage { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}