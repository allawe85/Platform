using Platform.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Blazor.Services.Auth
{
    public interface IApplicationUsersService
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task<ApplicationUser> CreateUserAsync(ApplicationUser user);
        Task<ApplicationUser?> UpdateUserAsync(string id, ApplicationUser user);
        Task<bool> DeleteUserAsync(string id);
    }
}
