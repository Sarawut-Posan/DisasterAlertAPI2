using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public class AlertSettingDto
{
    [Required(ErrorMessage = "RegionID is required")]
    public string RegionID { get; set; }

    [Required(ErrorMessage = "DisasterType is required")]
    public string DisasterType { get; set; }

    [Range(0, 100, ErrorMessage = "ThresholdScore must be between 0 and 100")]
    public int ThresholdScore { get; set; }
}