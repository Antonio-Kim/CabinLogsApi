using CabinLogsApi.DTO.Setting;
using CabinLogsApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CabinLogsApi.Controllers;

[ApiController]
[Route("settings")]
public class SettingsController : ControllerBase
{
    private readonly ISettingService _settingService;
    public SettingsController(ISettingService settingService)
    {
        _settingService = settingService;
    }

    [HttpGet(Name = "Get settings")]
    public async Task<IActionResult> GetSettings()
    {
        try
        {
            var settings = await _settingService.GetSettings();
            var data = settings.Select(s => new SettingDTO
            {
                Id = s.id,
                created_at = s.created_at,
                MinBookingLength = s.minBookingLength,
                MaxBookingLength = s.maxBookingLength,
                MaxGuestsPerBooking = s.maxGuestsPerBooking,
                BreakfastPrice = s.breakfastPrice
            }).ToList();
            return Ok(data);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve settings.");
        }
    }

    [HttpPut(Name = "Update Settings")]
    public async Task<IActionResult> UpdateSettings(Setting updatedSetting)
    {
        try
        {
            var settings = await _settingService.UpdateSettings(updatedSetting);
            return Ok(settings);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update settings.");
        }
    }
}