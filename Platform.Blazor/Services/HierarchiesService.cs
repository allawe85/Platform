using Platform.Data.DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Platform.Blazor.Services
{
    public class HierarchiesService : IHierarchiesService
    {
        private readonly HttpClient _http;

        public HierarchiesService(HttpClient http)
        {
            _http = http;
        }

        // Levels
        public async Task<List<HierarchyLevel>> GetLevelsAsync()
        {
            return await _http.GetFromJsonAsync<List<HierarchyLevel>>("api/HierarchyLevels");
        }

        public async Task<HierarchyLevel> GetLevelAsync(int id)
        {
            return await _http.GetFromJsonAsync<HierarchyLevel>($"api/HierarchyLevels/{id}");
        }

        public async Task<HierarchyLevel> CreateLevelAsync(HierarchyLevel level)
        {
            var response = await _http.PostAsJsonAsync("api/HierarchyLevels", level);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<HierarchyLevel>();
        }

        public async Task UpdateLevelAsync(int id, HierarchyLevel level)
        {
            var response = await _http.PutAsJsonAsync($"api/HierarchyLevels/{id}", level);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteLevelAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/HierarchyLevels/{id}");
            response.EnsureSuccessStatusCode();
        }

        // Hierarchies
        public async Task<List<Hierarchy>> GetHierarchiesAsync()
        {
            return await _http.GetFromJsonAsync<List<Hierarchy>>("api/Hierarchies");
        }

        public async Task<Hierarchy> GetHierarchyAsync(int id)
        {
            return await _http.GetFromJsonAsync<Hierarchy>($"api/Hierarchies/{id}");
        }

        public async Task<Hierarchy> CreateHierarchyAsync(Hierarchy hierarchy)
        {
            var response = await _http.PostAsJsonAsync("api/Hierarchies", hierarchy);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Hierarchy>();
        }

        public async Task UpdateHierarchyAsync(int id, Hierarchy hierarchy)
        {
            var response = await _http.PutAsJsonAsync($"api/Hierarchies/{id}", hierarchy);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteHierarchyAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/Hierarchies/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
