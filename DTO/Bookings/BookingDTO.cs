using CabinLogsApi.DTO.Cabins;
using CabinLogsApi.DTO.Guests;

namespace CabinLogsApi.DTO.Bookings;
public class BookingDTO
{
    public int Id { get; set; }
    public DateTime created_at { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberOfNights { get; set; }
    public int NumGuests { get; set; }
    public float CabinPrice { get; set; }
    public float ExtrasPrice { get; set; }
    public float TotalPrice { get; set; }
    public string? Status { get; set; }
    public bool HasBreakfast { get; set; }
    public bool IsPaid { get; set; }
    public string? Observations { get; set; }
    public int CabinId { get; set; }
    public int GuestId { get; set; }
    public CabinDTO? Cabin { get; set; }
    public GuestDTO? Guest { get; set; }
}
