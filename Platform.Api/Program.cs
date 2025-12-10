using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// Add PlatformDbContext as a di singltone service
builder.Services.AddDbContext<Platform.Data.PlatformDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformDbConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 1. Serves the generated JSON document (e.g., /swagger/v1/swagger.json)
    app.UseSwagger();

    // 2. Serves the interactive Swagger UI (e.g., /swagger)
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
