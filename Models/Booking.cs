using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CabinLogsApi.Models;

[Table("bookings")]
public class Booking
{
    [Required]
    [Key]
    public int id { get; set; }
    [Required]
    public DateTime created_at { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
    public int numberOfNights { get; set; }
    public int numGuests { get; set; }
    public float cabinPrice { get; set; }
    public float extrasPrice { get; set; }
    public float totalPrice { get; set; }
    public string? status { get; set; }
    public bool hasBreakfast { get; set; }
    public bool isPaid { get; set; }
    public string? observations { get; set; }

    [Required]
    public int cabinId { get; set; }
    [Required]
    public int guestId { get; set; }

    [ForeignKey("cabinId")]
    public Cabin? Cabin { get; set; }
    [ForeignKey("guestId")]
    public Guest? Guest { get; set; }
}
