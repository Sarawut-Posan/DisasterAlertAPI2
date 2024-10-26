using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Domain.DTO;
public class CreateAlertRequest
{
    [Required]
    public string RegionId { get; set; }

    [Required]
    public string DisasterType { get; set; }

    [Required]
    [EnumDataType(typeof(RiskLevel))]
    public RiskLevel RiskLevel { get; set; }

    [Required]
    public string Message { get; set; }

    // [MinLength(1)]
    // public List<string> RecipientPhoneNumbers { get; set; }
}