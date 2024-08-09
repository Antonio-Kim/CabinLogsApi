namespace CabinLogsApi.DTO.Cabins;

public class CabinDTO
{
    public int Id { get; set; }
    public DateTime created_at { get; set; } = DateTime.UtcNow;
    public string? Name { get; set; }
    public int MaxCapacity { get; set; }
    public int RegularPrice { get; set; }
    public int Discount { get; set; }
    public string? Description { get; set; }
}
