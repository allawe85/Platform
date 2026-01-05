using System.Net.Http.Json;
using Platform.Data.DTOs;

namespace Platform.Blazor.Services.Employees
{
    public class EmployeesService : IEmployeesService
    {
        private readonly HttpClient _http;

        public EmployeesService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<EmployeeInfo>> GetAllEmployeesAsync()
        {
            return await _http.GetFromJsonAsync<List<EmployeeInfo>>("api/employees") ?? new List<EmployeeInfo>();
        }

        public async Task<EmployeeInfo?> GetEmployeeInfoByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<EmployeeInfo>($"api/employees/{id}");
        }

        public Task<Employee?> GetEmployeeRecordByIdAsync(int id)
        {
             // Still partial implementation as per controller limitation for now
             return Task.FromResult<Employee?>(null); 
        }

        public async Task<Employee> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/employees", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Employee>();
        }

        public async Task<Employee?> UpdateEmployeeAsync(int id, UpdateEmployeeRequest request)
        {
            var response = await _http.PutAsJsonAsync($"api/employees/{id}", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Employee>();
            }
            return null;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/employees/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResetPasswordAsync(int id)
        {
            var response = await _http.PostAsync($"api/employees/{id}/reset-password", null);
            return response.IsSuccessStatusCode;
        }
    }
}
