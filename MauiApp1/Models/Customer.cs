using System.ComponentModel.DataAnnotations;

namespace MauiApp1.Models;

public sealed class Customer
{
    [Required]
    public  string Name { get; set; } = string.Empty;

    [Required]
    public  string Mobile { get; set; } = string.Empty;
    public  string? Address { get; set; }
}
