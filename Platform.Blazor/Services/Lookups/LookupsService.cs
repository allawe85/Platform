using Platform.Data.DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Platform.Blazor.Services.Lookups
{
    public class LookupsService : ILookupsService
    {
        private readonly HttpClient _http;

        public LookupsService(HttpClient http)
        {
            _http = http;
        }

        // Asset Statuses
        public async Task<List<AssetStatus>> GetAssetStatusesAsync()
        {
            return await _http.GetFromJsonAsync<List<AssetStatus>>("api/AssetStatuses");
        }

        public async Task<AssetStatus> CreateAssetStatusAsync(AssetStatus status)
        {
            var response = await _http.PostAsJsonAsync("api/AssetStatuses", status);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AssetStatus>();
        }

        public async Task UpdateAssetStatusAsync(int id, AssetStatus status)
        {
            var response = await _http.PutAsJsonAsync($"api/AssetStatuses/{id}", status);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAssetStatusAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/AssetStatuses/{id}");
            response.EnsureSuccessStatusCode();
        }

        // Asset Types
        public async Task<List<AssetType>> GetAssetTypesAsync()
        {
            return await _http.GetFromJsonAsync<List<AssetType>>("api/AssetTypes");
        }

        public async Task<AssetType> CreateAssetTypeAsync(AssetType type)
        {
            var response = await _http.PostAsJsonAsync("api/AssetTypes", type);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AssetType>();
        }

        public async Task UpdateAssetTypeAsync(int id, AssetType type)
        {
            var response = await _http.PutAsJsonAsync($"api/AssetTypes/{id}", type);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAssetTypeAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/AssetTypes/{id}");
            response.EnsureSuccessStatusCode();
        }

        // Document Types
        public async Task<List<DocumentType>> GetDocumentTypesAsync()
        {
            return await _http.GetFromJsonAsync<List<DocumentType>>("api/DocumentTypes");
        }

        public async Task<DocumentType> CreateDocumentTypeAsync(DocumentType type)
        {
            var response = await _http.PostAsJsonAsync("api/DocumentTypes", type);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<DocumentType>();
        }

        public async Task UpdateDocumentTypeAsync(int id, DocumentType type)
        {
            var response = await _http.PutAsJsonAsync($"api/DocumentTypes/{id}", type);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteDocumentTypeAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/DocumentTypes/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
