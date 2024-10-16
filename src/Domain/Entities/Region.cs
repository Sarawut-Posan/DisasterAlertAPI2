namespace Domain.Entities;

public class Region
{
    public int Id { get; set; }  
    public string RegionID { get; set; }  
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public List<string> DisasterTypes { get; set; }
}