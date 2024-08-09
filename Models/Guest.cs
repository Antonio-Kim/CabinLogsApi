using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CabinLogsApi.Models;

[Table("guests")]
public class Guest
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int id { get; set; }
    [Required]
    public DateTime created_at { get; set; }
    public string? fullName { get; set; }
    public string? email { get; set; }
    public string? nationalId { get; set; }
    public string? nationality { get; set; }
    public string? countryFlag { get; set; }

    public ICollection<Booking>? bookings { get; set; } = new List<Booking>();
}