using System.Net.Http.Json;

namespace Platform.Blazor.Services.Roles
{
    public interface IRolesService
    {
        Task<List<string>> GetRolesAsync();
    }

    public class RolesService : IRolesService
    {
         private readonly HttpClient _httpClient;

        public RolesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<string>> GetRolesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<string>>("api/roles") ?? new List<string>();
        }
    }
}
