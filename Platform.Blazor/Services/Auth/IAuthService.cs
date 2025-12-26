using Platform.Data.DTOs;

namespace Platform.Blazor.Services.Auth
{
    public interface IAuthService
    {
        Task<RegisterResult> RegisterAsync(RegisterRequest model);
        Task<string?> LoginAsync(LoginRequest model);
        Task LogoutAsync();
        Task<string?> GetTokenAsync();
    }
}