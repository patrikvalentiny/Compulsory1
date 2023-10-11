using System.ComponentModel.DataAnnotations;

namespace infrastructure.Models;

public class Material
{
    [Required]
    public int? Id { get; set; }
    [Required]
    public string? Name { get; set; }
}