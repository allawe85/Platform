using Microsoft.JSInterop;
using Platform.Data.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace Platform.Blazor.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private readonly AuthenticationStateProvider _authStateProvider;
        private const string TokenKey = "auth_token";

        public AuthService(HttpClient http, IJSRuntime js, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _js = js;
            _authStateProvider = authStateProvider;
        }

        public async Task<RegisterResult> RegisterAsync(RegisterRequest model)
        {
            var resp = await _http.PostAsJsonAsync("api/account/register", model);
            
            if (resp.IsSuccessStatusCode)
            {
                return new RegisterResult { Successful = true };
            }

            var details = await resp.Content.ReadFromJsonAsync<Dictionary<string, IEnumerable<string>>>();
            var errors = details?["errors"] ?? new List<string> { "An unknown error occurred." };
            
            return new RegisterResult { Successful = false, Errors = errors };
        }

        public async Task<string?> LoginAsync(LoginRequest model)
        {
            var resp = await _http.PostAsJsonAsync("api/account/login", model);
            if (!resp.IsSuccessStatusCode) return null;

            var auth = await resp.Content.ReadFromJsonAsync<AuthResponse>();
            if (auth?.Token != null)
            {
                await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, auth.Token);
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);
                ((ApiAuthenticationStateProvider)_authStateProvider).MarkUserAuthenticated(auth.Token);
                return auth.Token;
            }
            return null;
        }

        public async Task LogoutAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            _http.DefaultRequestHeaders.Authorization = null;
            ((ApiAuthenticationStateProvider)_authStateProvider).MarkUserLoggedOut();
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
        }
    }
}