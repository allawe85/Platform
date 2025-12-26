using Platform.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Blazor.Services.Documents
{
    public interface IDocumentsService
    {
        Task<List<Document>> GetAllDocumentsAsync();
        Task<Document?> GetDocumentByIdAsync(int id);
        Task<Document> CreateDocumentAsync(Document document);
        Task<Document?> UpdateDocumentAsync(int id, Document document);
        Task<bool> DeleteDocumentAsync(int id);
        Task<List<Document>> GetDocumentsByEmployeeIdAsync(int employeeId);
    }
}
