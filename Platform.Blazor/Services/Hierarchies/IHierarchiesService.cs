using Platform.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Blazor.Services.Hierarchies
{
    public interface IHierarchiesService
    {
        Task<List<HierarchyLevel>> GetLevelsAsync();
        Task<HierarchyLevel> GetLevelAsync(int id);
        Task<HierarchyLevel> CreateLevelAsync(HierarchyLevel level);
        Task UpdateLevelAsync(int id, HierarchyLevel level);
        Task DeleteLevelAsync(int id);

        Task<List<Hierarchy>> GetHierarchiesAsync();
        Task<Hierarchy> GetHierarchyAsync(int id);
        Task<Hierarchy> CreateHierarchyAsync(Hierarchy hierarchy);
        Task UpdateHierarchyAsync(int id, Hierarchy hierarchy);
        Task DeleteHierarchyAsync(int id);
    }
}
