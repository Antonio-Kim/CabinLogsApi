using CabinLogsApi.Models;
using Microsoft.EntityFrameworkCore;

public class SettingService : ISettingService
{
    private readonly ApplicationDbContext _context;
    public SettingService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Setting>> GetSettings()
    {
        try
        {
            var settings = await _context.Settings.ToListAsync();
            if (settings.Count == 0)
            {
                return new List<Setting>();
            }
            return settings;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error occurred when accessing Database: {ex.Message}");
        }
    }

    public async Task<List<Setting>> UpdateSettings(Setting setting)
    {
        try
        {
            var settings = await _context.Settings.ToListAsync();
            if (settings.Count == 0)
            {
                throw new InvalidOperationException("No settings found to update.");
            }
            var existingSetting = settings[0];
            existingSetting.minBookingLength = setting.minBookingLength;
            existingSetting.maxBookingLength = setting.maxBookingLength;
            existingSetting.maxGuestsPerBooking = setting.maxGuestsPerBooking;
            existingSetting.breakfastPrice = setting.breakfastPrice;

            await _context.SaveChangesAsync();

            return settings;

        }
        catch (Exception ex)
        {
            throw new Exception($"Error occurred when accessing Database: {ex.Message}");
        }
    }
}