using System.ComponentModel.DataAnnotations;

namespace infrastructure.Models;

public class BoxWithMaterialId
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
    public string? Location { get; set; }
    public string? Description { get; set; }
    public DateTime? Created { get; set; }
    public int? Quantity { get; set; }
    public int? MaterialId { get; set; }
}