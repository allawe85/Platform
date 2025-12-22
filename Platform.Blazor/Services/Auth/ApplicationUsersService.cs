using System.Net.Http.Json;
using Platform.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Blazor.Services.Auth
{
    public class ApplicationUsersService : IApplicationUsersService
    {
        private readonly HttpClient _http;

        public ApplicationUsersService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _http.GetFromJsonAsync<List<ApplicationUser>>("api/ApplicationUser") ?? new List<ApplicationUser>();
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await _http.GetFromJsonAsync<ApplicationUser>($"api/ApplicationUser/{id}");
        }

        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
        {
            var response = await _http.PostAsJsonAsync("api/ApplicationUser", user);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ApplicationUser>();
        }

        public async Task<ApplicationUser?> UpdateUserAsync(string id, ApplicationUser user)
        {
            var response = await _http.PutAsJsonAsync($"api/ApplicationUser/{id}", user);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApplicationUser>();
            }
            return null;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var response = await _http.DeleteAsync($"api/ApplicationUser/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
