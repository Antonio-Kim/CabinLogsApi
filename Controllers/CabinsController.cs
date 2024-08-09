using CabinLogsApi.DTO.Cabins;
using CabinLogsApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CabinLogsApi.Controllers;

[ApiController]
[Route("/cabins")]
public class CabinsController : ControllerBase
{
    private readonly ICabinService _cabinService;
    private readonly string _uploadPath;

    public CabinsController(ICabinService cabinService, IWebHostEnvironment env)
    {
        _uploadPath = Path.Combine(env.WebRootPath, "images");
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
        _cabinService = cabinService;
    }

    [HttpGet(Name = "Get a list of cabins")]
    public async Task<IActionResult> GetAllCabins()
    {
        try
        {
            var cabins = await _cabinService.GetCabins();
            var data = cabins.Select(c => new Cabin
            {
                id = c.id,
                created_at = c.created_at,
                name = c.name,
                maxCapacity = c.maxCapacity,
                regularPrice = c.regularPrice,
                discount = c.discount,
                description = c.description,
                image = c.image,
            }).ToList();

            return new OkObjectResult(data);
        }
        catch (Exception)
        {
            return StatusCode(500, "Failed to retrieve data from database.");
        }
    }

    [HttpGet("{id}", Name = "Get a cabin")]
    public async Task<IActionResult> GetCabin(int id)
    {
        try
        {
            var cabin = await _cabinService.GetCabin(id);
            if (cabin == null)
            {
                return StatusCode(404, "Cabin Id not found");
            }
            return new OkObjectResult(cabin);
        }
        catch (Exception)
        {
            return StatusCode(500, "Something went wrong");
        }

    }

    [HttpDelete("{id}", Name = "Remove a cabin")]
    public async Task<IActionResult> DeleteCabin(int id)
    {
        try
        {
            var cabin = await _cabinService.RemoveCabin(id);
            if (cabin == false)
            {
                return NotFound();
            }
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Something went wrong");
        }
    }

    [HttpPost(Name = "Add a cabin")]
    public async Task<IActionResult> AddCabin([FromForm] CabinDTO cabin, IFormFile? image)
    {
        if (cabin == null)
        {
            return BadRequest("Cabin is empty.");
        }

        string? imagePath = null;

        if (image != null && image.Length > 0)
        {
            var allowedExtensions = new[] { ".jpg", "jpeg", ".png" };
            var extensions = Path.GetExtension(image.FileName).ToLower();

            if (!allowedExtensions.Contains(extensions))
            {
                return BadRequest("Invalid file type. Only jpeg and png are allowed");
            }

            var fileName = Path.GetFileName(image.FileName);
            var filePath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            imagePath = $"/images/{fileName}";
        }

        var newCabin = new Cabin
        {
            created_at = DateTime.UtcNow,
            name = cabin.Name,
            maxCapacity = cabin.MaxCapacity,
            regularPrice = cabin.RegularPrice,
            discount = cabin.Discount,
            description = cabin.Description,
            image = imagePath
        };

        var result = await _cabinService.AddCabin(newCabin);
        if (result)
        {
            return CreatedAtAction(nameof(GetCabin), new { id = newCabin.id }, newCabin);
        }

        return Conflict("A cabin with same ID already exists.");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCabin(int id, [FromBody] Cabin updatedCabin)
    {
        if (updatedCabin == null)
        {
            return BadRequest("Cabin is empty.");
        }
        var result = await _cabinService.UpdateCabin(id, updatedCabin);
        if (result)
        {
            return NoContent();
        }

        return NotFound("Cabin not found.");
    }
}