using CabinLogsApi.Models;

public interface IGuestService
{
    public Task<List<Guest>> GetGuests();
    public Task<Guest?> GetGuest(int id);
}