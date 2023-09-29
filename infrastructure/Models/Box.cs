namespace infrastructure.Models;

public class Box
{
    public Guid Guid { get; set; } = Guid.NewGuid(); 
    public decimal[]? Measurements { get; set; } = new decimal[3];
    public string? Location { get; set; }
    public string? Description { get; set; }
}