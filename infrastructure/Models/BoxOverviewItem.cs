using System.ComponentModel.DataAnnotations;

namespace infrastructure.Models;

public class BoxOverviewItem
{
    public Guid? Guid { get; set; }
    [Required]
    [MinLength(3)]
    public string? Title { get; set; }
    [Required]
    [Range(0, 1000)]
    public decimal? Width { get; set; }
    [Required]
    [Range(0, 1000)]
    public decimal? Height { get; set; }
    [Required]
    [Range(0, 1000)]
    public decimal? Depth { get; set; }
    [Required]
    public int? Quantity { get; set; }
    public string? MaterialName { get; set; }
}