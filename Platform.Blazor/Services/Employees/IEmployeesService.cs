using Platform.Data.DTOs;

namespace Platform.Blazor.Services.Employees
{
    public interface IEmployeesService
    {
        Task<List<EmployeeInfo>> GetAllEmployeesAsync();
        Task<EmployeeInfo?> GetEmployeeInfoByIdAsync(int id);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<Employee?> UpdateEmployeeAsync(int id, Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<Employee?> GetEmployeeRecordByIdAsync(int id);
    }
}
