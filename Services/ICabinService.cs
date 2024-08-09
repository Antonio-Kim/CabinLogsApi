using CabinLogsApi.Models;

public interface ICabinService
{
    public Task<List<Cabin>> GetCabins();
    public Task<Cabin?> GetCabin(int id);
    public Task<bool> RemoveCabin(int id);
    public Task<bool> AddCabin(Cabin cabin);
    public Task<bool> UpdateCabin(int id, Cabin cabin);
}