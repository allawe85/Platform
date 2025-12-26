using Platform.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Blazor.Services.Assets
{
    public interface IAssetsService
    {
        Task<List<Asset>> GetAllAssetsAsync();
        Task<Asset?> GetAssetByIdAsync(int id);
        Task<Asset> CreateAssetAsync(Asset asset);
        Task<Asset?> UpdateAssetAsync(int id, Asset asset);
        Task<bool> DeleteAssetAsync(int id);
        Task<List<Asset>> GetAssetsByEmployeeIdAsync(int employeeId);
    }
}
