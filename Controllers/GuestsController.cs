using CabinLogsApi.DTO.Guests;
using CabinLogsApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CabinLogsApi.Controllers;

[ApiController]
[Route("/guests")]
public class GuestsController : ControllerBase
{
    private readonly IGuestService _guestService;
    public GuestsController(IGuestService guestService)
    {
        _guestService = guestService;
    }

    [HttpGet(Name = "Get a list of guests")]
    public async Task<IActionResult> GetAllGuests()
    {
        try
        {
            var guests = await _guestService.GetGuests();
            var data = guests.Select(g => new GuestDTO
            {
                Id = g.id,
                created_at = g.created_at,
                FullName = g.fullName,
                Email = g.email,
                NationalId = g.nationalId,
                Nationality = g.nationality,
                CountryFlag = g.countryFlag,
            }).ToList();
            return new OkObjectResult(data);
        }
        catch (Exception)
        {
            return StatusCode(505, "Failed to retrieve data from database.");
        }
    }

    [HttpGet("{id}", Name = "Get a guest")]
    public async Task<IActionResult> GetGuest(int id)
    {
        try
        {
            var guest = await _guestService.GetGuest(id);
            if (guest == null)
            {
                return new NotFoundObjectResult("Guest not found.");
            }
            return new OkObjectResult(guest);
        }
        catch (Exception)
        {
            return StatusCode(505, "Error retrieving guest.");
        }
    }
}
