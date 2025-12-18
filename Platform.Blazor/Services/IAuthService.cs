using Platform.Data.DTOs;

namespace Platform.Blazor.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequest model);
        Task<string?> LoginAsync(LoginRequest model);
        Task LogoutAsync();
        Task<string?> GetTokenAsync();
    }
}