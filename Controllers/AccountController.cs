using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CabinLogsApi.DTO;
using CabinLogsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CabinLogsApi.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApiUser> _userManager;
    private readonly SignInManager<ApiUser> _signInManager;

    public AccountController(
        ApplicationDbContext context,
        IConfiguration configuration,
        UserManager<ApiUser> userManager,
        SignInManager<ApiUser> signInManager
    )
    {
        _context = context;
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterDTO input)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApiUser()
                {
                    UserName = input.Email,
                    FullName = input.FullName,
                    Email = input.Email,
                };
                if (input.Email == null || input.Password == null)
                {
                    return BadRequest("Body requires email or password");
                }
                var result = await _userManager.CreateAsync(newUser, input.Password);

                if (result.Succeeded)
                {
                    return StatusCode(201, $"User has been created.");
                }
                else
                {
                    throw new Exception(string.Format("Error: {0}", string.Join(" ", result.Errors.Select(e => e.Description))));
                }
            }
            else
            {
                var details = new ValidationProblemDetails(ModelState);
                details.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                details.Status = StatusCodes.Status400BadRequest;
                return new BadRequestObjectResult(details);
            }
        }
        catch (Exception e)
        {
            var exceptionDetails = new ProblemDetails();
            exceptionDetails.Detail = e.Message;
            exceptionDetails.Status = StatusCodes.Status500InternalServerError;
            exceptionDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
            return StatusCode(StatusCodes.Status500InternalServerError, exceptionDetails);
        }
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginDTO input)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (input.Email == null || input.Password == null)
                {
                    return BadRequest("Email and password are required.");
                }
                var user = await _userManager.FindByEmailAsync(input.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, input.Password))
                    return Unauthorized("Invalid login attempt.");
                else
                {
                    var signingKey = _configuration["JWT:SigningKey"];
                    if (signingKey == null)
                    {
                        throw new InvalidOperationException("JWT SigningKey is not configured.");
                    }
                    var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(signingKey)),
                    SecurityAlgorithms.HmacSha256);

                    if (user.Email == null)
                    {
                        throw new InvalidOperationException("Password is required.");
                    }

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id)
                    };
                    claims.Add(new Claim(ClaimTypes.Email, user.Email));

                    var jwtObject = new JwtSecurityToken(
                        issuer: _configuration["JWT:Issuer"],
                        audience: _configuration["JWT:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddSeconds(300),
                        signingCredentials: signingCredentials);

                    var jwtString = new JwtSecurityTokenHandler()
                        .WriteToken(jwtObject);

                    var userSession = new
                    {
                        token = jwtString,
                        fullName = user.UserName,
                        email = user.Email,
                        id = user.Id,
                    };

                    return StatusCode(StatusCodes.Status200OK, userSession);
                }
            }
            else
            {
                var details = new ValidationProblemDetails(ModelState);
                details.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                details.Status = StatusCodes.Status400BadRequest;
                return new BadRequestObjectResult(details);
            }
        }
        catch (Exception e)
        {
            var exceptionDetails = new ProblemDetails();
            exceptionDetails.Detail = e.Message;
            exceptionDetails.Status = StatusCodes.Status401Unauthorized;
            exceptionDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
            return StatusCode(StatusCodes.Status401Unauthorized, exceptionDetails);
        }
    }

    [HttpGet(Name = "Get user info")]
    [Authorize]
    public async Task<IActionResult> Info()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            // Use UserManager to find the user by ID
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Return user details
            var userInfo = new
            {
                user.Id,
                user.FullName,
                user.Email
            };

            return Ok(userInfo);
        }
        catch (Exception e)
        {
            var exceptionDetails = new ProblemDetails
            {
                Detail = e.Message,
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };
            return StatusCode(StatusCodes.Status500InternalServerError, exceptionDetails);
        }
    }

    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateDTO input)
    {
        if (input == null)
        {
            return BadRequest("User data is required");
        }
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        bool hasChanges = false;
        if (!string.IsNullOrEmpty(input.FullName))
        {
            user.FullName = input.FullName;
            hasChanges = true;
        }

        if (!string.IsNullOrEmpty(input.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, input.Password);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to update password.");
            }
            hasChanges = true;
        }

        if (hasChanges)
        {
            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                var updatedUser = new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.FullName,
                };
                return Ok(updatedUser);
            }
            return BadRequest("Failed to update user information");
        }
        return BadRequest("No data to update");
    }
}