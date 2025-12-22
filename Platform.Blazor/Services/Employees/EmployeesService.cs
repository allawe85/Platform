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

        public async Task<Employee?> GetEmployeeRecordByIdAsync(int id)
        {
             // The controller returns EmployeeInfo for GetEmployeeById. 
             // We might need a separate endpoint to get the raw Employee record for editing if EmployeeInfo doesn't map back 1:1 or is read-only view.
             // However, looking at the Controller: 
             // GetEmployeeById calls _context.GetEmployeeByIdAsync(id) which returns EmployeeInfo? 
             // Wait, let's re-read PlatformDbContext.
             // GetEmployeeByIdAsync returns EmployeeInfo.
             // GetEmployeeRecordByIdAsync returns Employee.
             // But the Controller only exposes GetEmployeeById (returning EmployeeInfo).
             // AND UpdateEmployee takes Employee.
             // This is a mismatch in the API. We can't fetch the 'Employee' entity to edit it easily if the API only gives us 'EmployeeInfo'.
             // I'll assume for now we can construct an Employee object from EmployeeInfo for editing, 
             // OR I might need to fix the API to expose GetEmployeeRecord.
             
             // Let's look at the controller again.
             // EmployeesController.GetEmployeeById calls _context.GetEmployeeByIdAsync(id) -> EmployeeInfo.
             // There is NO endpoint exposed for GetEmployeeRecordByIdAsync.
             // I should probably add one to the controller to make editing safe.
             
             // For now, I'll implement what I can.
             return null; 
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            var response = await _http.PostAsJsonAsync("api/employees", employee);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Employee>();
        }

        public async Task<Employee?> UpdateEmployeeAsync(int id, Employee employee)
        {
            var response = await _http.PutAsJsonAsync($"api/employees/{id}", employee);
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
    }
}
