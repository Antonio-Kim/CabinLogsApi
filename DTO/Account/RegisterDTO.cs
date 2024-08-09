using System.ComponentModel.DataAnnotations;

namespace CabinLogsApi.DTO;

public class RegisterDTO
{
    [Required]
    [MaxLength(255)]
    public string? FullName { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
}