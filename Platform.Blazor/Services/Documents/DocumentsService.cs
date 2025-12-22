using System.Net.Http.Json;
using Platform.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Blazor.Services.Documents
{
    public class DocumentsService : IDocumentsService
    {
        private readonly HttpClient _http;

        public DocumentsService(HttpClient http)
        {
            _http = http;
        }

        // Note: Controller route is "api/Document" so I should use "api/Document" not "api/documents"
        
        public async Task<List<Document>> GetAllDocumentsAsync()
        {
            return await _http.GetFromJsonAsync<List<Document>>("api/Document") ?? new List<Document>();
        }

        public async Task<Document?> GetDocumentByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<Document>($"api/Document/{id}");
        }

        public async Task<Document> CreateDocumentAsync(Document document)
        {
            var response = await _http.PostAsJsonAsync("api/Document", document);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Document>();
        }

        public async Task<Document?> UpdateDocumentAsync(int id, Document document)
        {
            var response = await _http.PutAsJsonAsync($"api/Document/{id}", document);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Document>();
            }
            return null;
        }

        public async Task<bool> DeleteDocumentAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/Document/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Document>> GetDocumentsByEmployeeIdAsync(int employeeId)
        {
            return await _http.GetFromJsonAsync<List<Document>>($"api/Document/ByEmployee/{employeeId}") ?? new List<Document>();
        }
    }
}
