using CabinLogsApi.Models;
using Microsoft.EntityFrameworkCore;

public class BookingService : IBookingService
{
    private readonly ApplicationDbContext _context;
    public BookingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RemoveBooking(int id)
    {
        try
        {
            var booking = await GetBooking(id);
            if (booking == null)
            {
                return false;
            }
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            throw new Exception("Error occured while trying remove booking");
        }

    }

    public async Task<Booking?> GetBooking(int id)
    {
        try
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.id == id);
            return booking ?? null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error occurred when trying to access databse: {ex.Message}");
        }
    }

    public async Task<List<Booking>> GetBookings()
    {
        try
        {
            var bookings = await _context.Bookings.ToListAsync();
            if (bookings.Count == 0)
            {
                return new List<Booking>();
            }

            return bookings;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error occurred when trying to access database: {ex.Message}");
        }
    }

    public async Task SaveBookingAsync(Booking booking)
    {
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }
}