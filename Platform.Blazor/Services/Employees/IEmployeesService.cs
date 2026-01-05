using Platform.Data.DTOs;

namespace Platform.Blazor.Services.Employees
{
    public interface IEmployeesService
    {
        Task<List<EmployeeInfo>> GetAllEmployeesAsync();
        Task<EmployeeInfo?> GetEmployeeInfoByIdAsync(int id);
        Task<Employee> CreateEmployeeAsync(CreateEmployeeRequest request);
        Task<Employee?> UpdateEmployeeAsync(int id, UpdateEmployeeRequest request);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<Employee?> GetEmployeeRecordByIdAsync(int id);
        Task<bool> ResetPasswordAsync(int id);
    }
}
