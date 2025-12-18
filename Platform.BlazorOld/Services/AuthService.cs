using Microsoft.JSInterop;
using Platform.Api.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Platform.Blazor.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private const string TokenKey = "auth_token";

        public AuthService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        public async Task<bool> RegisterAsync(RegisterRequest model)
        {
            var resp = await _http.PostAsJsonAsync("api/account/register", model);
            return resp.IsSuccessStatusCode;
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
                return auth.Token;
            }
            return null;
        }

        public async Task LogoutAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            _http.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
        }
    }
}