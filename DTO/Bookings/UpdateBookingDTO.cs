namespace CabinLogsApi.DTO.Bookings;
public class UpdateBookingDTO
{
    public string? Status { get; set; }
    public bool? IsPaid { get; set; }
    public float? ExtrasPrice { get; set; }
    public bool? HasBreakfast { get; set; }
    public float? TotalPrice { get; set; }
}

