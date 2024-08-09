using Microsoft.AspNetCore.Identity;

namespace CabinLogsApi.Models;

public class ApiUser : IdentityUser
{
    public string? FullName { get; set; }
}