using Platform.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Blazor.Services.Lookups
{
    public interface ILookupsService
    {
        // Asset Statuses
        Task<List<AssetStatus>> GetAssetStatusesAsync();
        Task<AssetStatus> CreateAssetStatusAsync(AssetStatus status);
        Task UpdateAssetStatusAsync(int id, AssetStatus status);
        Task DeleteAssetStatusAsync(int id);

        // Asset Types
        Task<List<AssetType>> GetAssetTypesAsync();
        Task<AssetType> CreateAssetTypeAsync(AssetType type);
        Task UpdateAssetTypeAsync(int id, AssetType type);
        Task DeleteAssetTypeAsync(int id);

        // Document Types
        Task<List<DocumentType>> GetDocumentTypesAsync();
        Task<DocumentType> CreateDocumentTypeAsync(DocumentType type);
        Task UpdateDocumentTypeAsync(int id, DocumentType type);
        Task DeleteDocumentTypeAsync(int id);
    }
}
