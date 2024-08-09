using CabinLogsApi.DTO.Bookings;
using CabinLogsApi.DTO.Cabins;
using CabinLogsApi.DTO.Guests;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace CabinLogsApi.Controllers;

[ApiController]
[Route("/bookings")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly ICabinService _cabinService;
    private readonly IGuestService _guestService;
    public BookingsController(IBookingService bookingService, ICabinService cabinService, IGuestService guestService)
    {
        _bookingService = bookingService;
        _cabinService = cabinService;
        _guestService = guestService;
    }

    [HttpGet(Name = "Get All Bookings")]
    public async Task<IActionResult> GetBookings(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? options = "startDate",
        [FromQuery] string? sortOrder = "asc",
        [FromQuery] string? status = null,
        [FromQuery] int? created = null,
        [FromQuery] int? start = null)
    {
        try
        {
            var bookingsList = await _bookingService.GetBookings();
            var query = bookingsList.AsQueryable();
            if (string.IsNullOrWhiteSpace(options))
            {
                options = "startDate";
            }

            if (created.HasValue)
            {
                var endDate = DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);
                var startDate = DateTime.UtcNow.AddDays(-created.Value);
                query = query.Where(b => b.created_at >= startDate && b.created_at <= endDate);
            }

            if (start.HasValue)
            {
                var endDate = DateTime.UtcNow;
                var startDate = DateTime.UtcNow.AddDays(-start.Value);
                query = query.Where(b => b.startDate >= startDate && b.startDate <= endDate);
            }

            var totalCount = query.Count();

            if (!string.IsNullOrEmpty(status))
            {
                if (status != "all")
                {
                    query = query.Where(b => (b.status ?? string.Empty).Contains(status));
                }
            }

            query = query.OrderBy($"{options} {sortOrder}");
            var bookings = query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            var cabinTasks = bookings.Select(b => _cabinService.GetCabin(b.cabinId)).ToArray();
            var guestTasks = bookings.Select(b => _guestService.GetGuest(b.guestId)).ToArray();

            var cabins = await Task.WhenAll(cabinTasks);
            var guests = await Task.WhenAll(guestTasks);
            var data = bookings.Select(b => new BookingDTO
            {
                Id = b.id,
                created_at = b.created_at,
                StartDate = b.startDate,
                EndDate = b.endDate,
                NumberOfNights = b.numberOfNights,
                NumGuests = b.numGuests,
                CabinPrice = b.cabinPrice,
                ExtrasPrice = b.extrasPrice,
                TotalPrice = b.totalPrice,
                Status = b.status,
                HasBreakfast = b.hasBreakfast,
                IsPaid = b.isPaid,
                Observations = b.observations,
                CabinId = b.cabinId,
                GuestId = b.guestId,
                Cabin = cabins.FirstOrDefault(c => c != null && c.id == b.cabinId) is { } cabin ? new CabinDTO
                {
                    Id = cabin.id,
                    created_at = cabin.created_at,
                    Name = cabin.name,
                    MaxCapacity = cabin.maxCapacity,
                    RegularPrice = cabin.regularPrice,
                    Discount = cabin.discount,
                    Description = cabin.description
                } : null,
                Guest = guests.FirstOrDefault(g => g != null && g.id == b.guestId) is { } guest ? new GuestDTO
                {
                    Id = guest.id,
                    created_at = guest.created_at,
                    FullName = guest.fullName,
                    Email = guest.email,
                    NationalId = guest.nationalId,
                    Nationality = guest.nationality,
                    CountryFlag = guest.countryFlag
                } : null
            }).ToList();
            return Ok(new
            {
                TotalCount = totalCount,
                Bookings = data
            });
        }
        catch (Exception)
        {
            return StatusCode(500, "Failed to retrieve data from database.");
        }
    }

    [HttpGet("{id}", Name = "Get booking from id")]
    public async Task<IActionResult> GetBooking(int id)
    {
        try
        {
            var booking = await _bookingService.GetBooking(id);
            if (booking == null)
            {
                return StatusCode(404, "Booking not found.");
            }
            var cabin = await _cabinService.GetCabin(booking.cabinId);
            var guest = await _guestService.GetGuest(booking.guestId);

            var bookingDTO = new BookingDTO
            {
                Id = booking.id,
                created_at = booking.created_at,
                StartDate = booking.startDate,
                EndDate = booking.endDate,
                NumberOfNights = booking.numberOfNights,
                NumGuests = booking.numGuests,
                CabinPrice = booking.cabinPrice,
                ExtrasPrice = booking.extrasPrice,
                TotalPrice = booking.totalPrice,
                Status = booking.status,
                HasBreakfast = booking.hasBreakfast,
                IsPaid = booking.isPaid,
                Observations = booking.observations,
                CabinId = booking.cabinId,
                GuestId = booking.guestId,
                Cabin = cabin != null ? new CabinDTO
                {
                    Id = cabin.id,
                    created_at = cabin.created_at,
                    Name = cabin.name,
                    MaxCapacity = cabin.maxCapacity,
                    RegularPrice = cabin.regularPrice,
                    Discount = cabin.discount,
                    Description = cabin.description,
                } : null,
                Guest = guest != null ? new GuestDTO
                {
                    Id = guest.id,
                    created_at = guest.created_at,
                    FullName = guest.fullName,
                    Email = guest.email,
                    NationalId = guest.nationalId,
                    Nationality = guest.nationality,
                    CountryFlag = guest.countryFlag,
                } : null,
            };
            return new OkObjectResult(bookingDTO);
        }
        catch (Exception)
        {
            return StatusCode(500, "Failed to retrive booking from database.");
        }
    }

    [HttpPatch("{id}", Name = "Updating booking")]
    public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingDTO updateStatus)
    {
        if (updateStatus == null)
        {
            return BadRequest("Nothing is being updated.");
        }

        var validStatus = new HashSet<string> { "confirmed", "unconfirmed", "checked-in", "checked-out" };
        if (updateStatus.Status != null && !validStatus.Contains(updateStatus.Status))
        {
            return BadRequest("Invalid status value");
        }

        try
        {
            var booking = await _bookingService.GetBooking(id);
            if (booking == null)
            {
                return NotFound($"Booking Id {id} not found.");
            }

            if (updateStatus.Status != null) booking.status = updateStatus.Status;
            if (updateStatus.IsPaid.HasValue) booking.isPaid = updateStatus.IsPaid.Value;
            if (updateStatus.HasBreakfast.HasValue) booking.hasBreakfast = updateStatus.HasBreakfast.Value;
            if (updateStatus.ExtrasPrice.HasValue) booking.extrasPrice = updateStatus.ExtrasPrice.Value;
            if (updateStatus.TotalPrice.HasValue) booking.totalPrice = updateStatus.TotalPrice.Value;

            await _bookingService.SaveBookingAsync(booking);

            var cabin = await _cabinService.GetCabin(booking.cabinId);
            var guest = await _guestService.GetGuest(booking.guestId);

            var response = new BookingDTO
            {
                Id = booking.id,
                created_at = booking.created_at,
                StartDate = booking.startDate,
                EndDate = booking.endDate,
                NumberOfNights = booking.numberOfNights,
                NumGuests = booking.numGuests,
                CabinPrice = booking.cabinPrice,
                ExtrasPrice = booking.extrasPrice,
                TotalPrice = booking.totalPrice,
                Status = booking.status,
                HasBreakfast = booking.hasBreakfast,
                IsPaid = booking.isPaid,
                Observations = booking.observations,
                CabinId = booking.cabinId,
                GuestId = booking.guestId,
                Cabin = cabin != null ? new CabinDTO
                {
                    Id = cabin.id,
                    created_at = cabin.created_at,
                    Name = cabin.name,
                    MaxCapacity = cabin.maxCapacity,
                    RegularPrice = cabin.regularPrice,
                    Discount = cabin.discount,
                    Description = cabin.description,
                } : null,
                Guest = guest != null ? new GuestDTO
                {
                    Id = guest.id,
                    created_at = guest.created_at,
                    FullName = guest.fullName,
                    Email = guest.email,
                    NationalId = guest.nationalId,
                    Nationality = guest.nationality,
                    CountryFlag = guest.countryFlag,
                } : null,
            };

            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred while updating booking.");
        }
    }

    [HttpDelete("{id}", Name = "Delete booking")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        try
        {
            var result = await _bookingService.RemoveBooking(id);
            if (result == false)
            {
                return NotFound($"Could not find booking with id {id}");
            }
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not delete booking");
        }
    }
}
