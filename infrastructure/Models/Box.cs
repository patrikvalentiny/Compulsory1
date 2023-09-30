namespace infrastructure.Models;

public class Box
{
    public Guid? Guid { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal? Depth { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
}