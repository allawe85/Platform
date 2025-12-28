using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Components.Authorization;
using Platform.Blazor;
using MudBlazor.Services;
using System.Net.Http;
using Platform.Blazor.Services.Auth;
using Platform.Blazor.Services.Hierarchies;
using Platform.Blazor.Services.Lookups;
using Microsoft.JSInterop;
using Platform.Blazor.Services.Employees;
using Platform.Blazor.Services.Assets;
using Platform.Blazor.Services.Documents;
using Platform.Blazor.Services.Events;
using Platform.Blazor.Services;
using Platform.Blazor.Services.Polls;
using Platform.Blazor.Services.TimeAttendance;
using Platform.Blazor.Services.Announcements;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddLocalization();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// API base
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5228/") });

// Auth + state provider
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices();
builder.Services.AddScoped<LayoutService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IHierarchiesService, HierarchiesService>();
builder.Services.AddScoped<ILookupsService, LookupsService>();
builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<ApiAuthenticationStateProvider>());

builder.Services.AddScoped<IApplicationUsersService, ApplicationUsersService>();
builder.Services.AddScoped<IEmployeesService, EmployeesService>();
builder.Services.AddScoped<IAssetsService, AssetsService>();
builder.Services.AddScoped<IDocumentsService, DocumentsService>();
builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<IPollsService, PollsService>();
builder.Services.AddScoped<Platform.Blazor.Services.Leaves.LeaveService>();
builder.Services.AddScoped<ITimeAttendanceService, TimeAttendanceService>();
builder.Services.AddScoped<IAnnouncementsService, AnnouncementsService>();


var host = builder.Build();

var js = host.Services.GetRequiredService<IJSRuntime>();
var culture = new CultureInfo("en-US");
try
{
    var result = await js.InvokeAsync<string>("localStorage.getItem", "culture");
    if (!string.IsNullOrEmpty(result))
    {
        culture = new CultureInfo(result);
    }
}
catch (Exception)
{
    // Fallback to default culture if access fails
}

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();
