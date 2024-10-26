using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public class CreateRegionDto
{
    [Required]
    public string RegionID { get; set; }
    
    [Required]
    [Range(-90, 90)]
    public double Latitude { get; set; }
    
    [Required]
    [Range(-180, 180)]
    public double Longitude { get; set; }
    
    [Required]
    public List<string> DisasterTypes { get; set; }
}