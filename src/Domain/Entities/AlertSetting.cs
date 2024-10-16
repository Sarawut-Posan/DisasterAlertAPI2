namespace Domain.Entities;

public class AlertSetting
{
    public int Id { get; set; }
    public int RegionId { get; set; }
    public string DisasterType { get; set; }
    public double ThresholdScore { get; set; }
}