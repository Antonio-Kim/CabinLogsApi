using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CabinLogsApi.Models;

[Table("cabins")]
public class Cabin
{
    [Required]
    [Key]
    public int id { get; set; }
    [Required]
    public DateTime created_at { get; set; }
    public string? name { get; set; }
    public int maxCapacity { get; set; }
    public int regularPrice { get; set; }
    public int discount { get; set; }
    public string? description { get; set; }
    public string? image { get; set; }

    public ICollection<Booking>? bookings { get; set; } = new List<Booking>();
}