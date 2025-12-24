using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Platform.Blazor;
using MudBlazor.Services;
using System.Net.Http;
using Platform.Blazor.Services.Auth;
using Platform.Blazor.Services.Hierarchies;
using Platform.Blazor.Services.Lookups;
using Platform.Blazor.Services.Employees;
using Platform.Blazor.Services.Assets;
using Platform.Blazor.Services.Documents;
using Platform.Blazor.Services.Events;
using Platform.Blazor.Services.Polls;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// API base
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5228/") });

// Auth + state provider
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices();
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

await builder.Build().RunAsync();
