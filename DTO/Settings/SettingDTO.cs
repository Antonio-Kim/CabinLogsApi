namespace CabinLogsApi.DTO.Setting;

public class SettingDTO
{
    public int Id { get; set; }
    public DateTime created_at { get; set; }
    public int MinBookingLength { get; set; }
    public int MaxBookingLength { get; set; }
    public int MaxGuestsPerBooking { get; set; }
    public float BreakfastPrice { get; set; }
}