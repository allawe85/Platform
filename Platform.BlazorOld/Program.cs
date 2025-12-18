using Platform.Blazor.Components;
using Platform.Blazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Remove the experimental AddRazorComponents() unless you intentionally use that model.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// HttpClient for server-side use (use actual API base)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5228/") }); // API base
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

// Map endpoints required for Razor Pages + Blazor Server
app.MapRazorPages();    // <-- required so MapFallbackToPage("/_Host") can find the page
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();


/*
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") }); // API base
builder.Services.AddScoped<IAuthService, AuthService>();

await builder.Build().RunAsync();
*/