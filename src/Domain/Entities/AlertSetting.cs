namespace Domain.Entities;

public class AlertSetting
{
    public int Id { get; set; }
    public string RegionId { get; set; }
    public string DisasterType { get; set; }
    public int ThresholdScore { get; set; }
}