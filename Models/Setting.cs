using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CabinLogsApi.Models;

[Table("settings")]
public class Setting
{
    [Required]
    [Key]
    public int id { get; set; }
    [Required]
    public DateTime created_at { get; set; }
    public int minBookingLength { get; set; }
    public int maxBookingLength { get; set; }
    public int maxGuestsPerBooking { get; set; }
    public float breakfastPrice { get; set; }
}