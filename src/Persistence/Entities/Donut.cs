using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;

public class Donut
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(32)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(128)]
    public string? Description { get; set; }

    [Precision(5, 2)]
    public decimal Price { get; set; }
}
