using CabinLogsApi.Constants;
using CabinLogsApi.Models;
using CabinLogsApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICabinService, CabinService>();
builder.Services.AddScoped<IGuestService, GuestService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ISeedDataService, SeedDataService>();

// Adding Identity service
builder.Services.AddIdentity<ApiUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme =
        JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var issuer = builder.Configuration["JWT:Issuer"];
    var audience = builder.Configuration["JWT:Audience"];
    var signingKey = builder.Configuration["JWT:SigningKey"];

    if (string.IsNullOrEmpty(signingKey))
    {
        throw new InvalidOperationException("JWT SigningKey is not configured.");
    }
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(signingKey)
        )
    };
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(cfg =>
    {
        var allowedOrigin = builder.Configuration["AllowedOrigins"] ?? "*";
        cfg.WithOrigins(allowedOrigin);
        cfg.AllowAnyHeader();
        cfg.AllowAnyMethod();
    });
    options.AddPolicy(name: "webapi",
        cfg =>
        {
            cfg.AllowAnyOrigin();
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();

    var seedDataService = scope.ServiceProvider.GetRequiredService<ISeedDataService>();
    await seedDataService.SeedRolesAsync();
}

app.MapGet("/auth/test/1",
[Authorize(Roles = RoleNames.User)]
[ResponseCache(NoStore = true)]
() =>
{
    return Results.Ok("Hello, User!");
});

app.MapGet("/auth/test/2",
[Authorize(Roles = RoleNames.Administrator)]
[ResponseCache(NoStore = true)]
() =>
{
    return Results.Ok("Hello, Administrator!");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("webapi");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
