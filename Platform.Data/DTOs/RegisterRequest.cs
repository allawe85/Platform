namespace Platform.Data.DTOs
{
    // Mutable DTOs so Blazor @bind and other client code can set properties.
    public record RegisterRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public RegisterRequest() { }
        public RegisterRequest(string userName, string email, string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }
    }

    public record LoginRequest
    {
        public string UserNameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public LoginRequest() { }
        public LoginRequest(string userNameOrEmail, string password)
        {
            UserNameOrEmail = userNameOrEmail;
            Password = password;
        }
    }

    public record AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public AuthResponse() { }
        public AuthResponse(string token, string userName, string email)
        {
            Token = token;
            UserName = userName;
            Email = email;
        }
    }
}