using CabinLogsApi.Models;
using Microsoft.EntityFrameworkCore;

public class CabinService : ICabinService
{
    private readonly ApplicationDbContext _context;

    public CabinService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddCabin(Cabin cabin)
    {
        var checkCabin = await _context.Cabins.FindAsync(cabin.id);
        if (checkCabin != null)
        {
            return false;
        }

        try
        {
            await _context.Cabins.AddAsync(cabin);
            _context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            throw new Exception("Error occurred when adding cabin.");
        }
    }

    public async Task<Cabin?> GetCabin(int id)
    {
        var cabin = await _context.Cabins.FirstOrDefaultAsync(c => c.id == id);
        return cabin;
    }

    public async Task<List<Cabin>> GetCabins()
    {
        try
        {
            var cabins = await _context.Cabins.ToListAsync();
            if (cabins.Count == 0)
            {
                return new List<Cabin>();
            }
            return cabins;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error occurred when accessing Database: {ex.Message}");
        }
    }

    public async Task<bool> RemoveCabin(int id)
    {
        try
        {
            var cabin = await _context.Cabins.FindAsync(id);
            if (cabin == null)
            {
                return false;
            }
            _context.Cabins.Remove(cabin);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            throw new Exception("Error occurred when removing cabin.");
        }
    }

    public async Task<bool> UpdateCabin(int id, Cabin updatedCabin)
    {
        var existingCabin = await _context.Cabins.FindAsync(id);
        if (existingCabin == null)
        {
            return false;
        }

        existingCabin.name = updatedCabin.name;
        existingCabin.maxCapacity = updatedCabin.maxCapacity;
        existingCabin.regularPrice = updatedCabin.regularPrice;
        existingCabin.discount = updatedCabin.discount;
        existingCabin.description = updatedCabin.description;
        existingCabin.image = updatedCabin.image;
        existingCabin.created_at = updatedCabin.created_at;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred when updating cabin.", ex);
        }
    }
}