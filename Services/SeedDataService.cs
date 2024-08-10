
using CabinLogsApi.Constants;
using Microsoft.AspNetCore.Identity;

namespace CabinLogsApi.Services;

public class SeedDataService : ISeedDataService
{
	private readonly RoleManager<IdentityRole> _roleManager;

	public SeedDataService(RoleManager<IdentityRole> roleManager)
	{
		_roleManager = roleManager;
	}

	public async Task SeedRolesAsync()
	{
		var roles = new[]
		{
			RoleNames.User,
			RoleNames.Administrator
		};

		foreach (var role in roles)
		{
			if (!await _roleManager.RoleExistsAsync(role))
			{
				await _roleManager.CreateAsync(new IdentityRole(role));
			}
		}
	}
}