using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Platform.Api.Services;
using Platform.Data;
using Platform.Data.DTOs;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add PlatformDbContext as a di singltone service --> this why we did not implement singltone logic in PlatformDbContext class
builder.Services.AddDbContext<Platform.Data.PlatformDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformDbConnection")));

#region for Identity and JWT

// Identity + EF store (assuming PlatformDbContext is already configured)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<PlatformDbContext>()
    .AddDefaultTokenProviders();

// JWT configuration
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key missing");
var issuer = builder.Configuration["Jwt:Issuer"] ?? "platform";
var audience = builder.Configuration["Jwt:Audience"] ?? "platform_clients";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateLifetime = true
    };
});

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
// Allow the Blazor client origin (adjust port/origin)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorDev", policy =>
    {
        policy.WithOrigins("http://localhost:5000", "http://localhost:5228") // Blazor client origin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
#endregion

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 1. Serves the generated JSON document (e.g., /swagger/v1/swagger.json)
    app.UseSwagger();

    // 2. Serves the interactive Swagger UI (e.g., /swagger)
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("AllowBlazorDev");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
