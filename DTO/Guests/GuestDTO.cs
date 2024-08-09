namespace CabinLogsApi.DTO.Guests;

public class GuestDTO
{
    public int Id { get; set; }
    public DateTime created_at { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? NationalId { get; set; }
    public string? Nationality { get; set; }
    public string? CountryFlag { get; set; }
}