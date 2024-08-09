using System.Data.Common;
using CabinLogsApi.Models;
using Microsoft.EntityFrameworkCore;

public class GuestService : IGuestService
{
    private readonly ApplicationDbContext _context;
    public GuestService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guest?> GetGuest(int id)
    {
        var guest = await _context.Guests.FirstOrDefaultAsync(g => g.id == id);
        return guest;
    }

    public async Task<List<Guest>> GetGuests()
    {
        try
        {
            var guests = await _context.Guests.ToListAsync();
            if (guests.Count == 0)
            {
                return new List<Guest>();
            }
            return guests;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error occurred when accessing Database: {ex.Message}");
        }
    }
}