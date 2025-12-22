using System.Net.Http.Json;
using Platform.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Blazor.Services.Assets
{
    public class AssetsService : IAssetsService
    {
        private readonly HttpClient _http;

        public AssetsService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Asset>> GetAllAssetsAsync()
        {
            return await _http.GetFromJsonAsync<List<Asset>>("api/assets") ?? new List<Asset>();
        }

        public async Task<Asset?> GetAssetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<Asset>($"api/assets/{id}");
        }

        public async Task<Asset> CreateAssetAsync(Asset asset)
        {
            var response = await _http.PostAsJsonAsync("api/assets", asset);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Asset>();
        }

        public async Task<Asset?> UpdateAssetAsync(int id, Asset asset)
        {
            var response = await _http.PutAsJsonAsync($"api/assets/{id}", asset);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Asset>();
            }
            return null;
        }

        public async Task<bool> DeleteAssetAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/assets/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Asset>> GetAssetsByEmployeeIdAsync(int employeeId)
        {
            return await _http.GetFromJsonAsync<List<Asset>>($"api/assets/ByEmployee/{employeeId}") ?? new List<Asset>();
        }
    }
}
