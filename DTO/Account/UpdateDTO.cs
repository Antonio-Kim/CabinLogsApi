using System.ComponentModel.DataAnnotations;

namespace CabinLogsApi.DTO;

public class UpdateDTO
{
    [MaxLength(255)]
    public string? FullName { get; set; }
    public string? Password { get; set; }
}