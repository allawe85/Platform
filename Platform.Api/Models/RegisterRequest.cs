namespace Platform.Api.Models
{
    public record RegisterRequest(string UserName, string Email, string Password);
    public record LoginRequest(string UserNameOrEmail, string Password);
    public record AuthResponse(string Token, string UserName, string Email);
}