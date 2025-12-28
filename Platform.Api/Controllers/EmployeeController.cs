using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.Data;
using Platform.Data.DTOs;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly PlatformDbContext _context;

        public EmployeesController(PlatformDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _context.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _context.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Add Employee
                var employee = request.Employee;
                var createdEmployeeEntry = await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();

                var createdEmployee = createdEmployeeEntry.Entity;

                // 2. Add Leave Balances if provided
                if (request.LeaveBalances != null && request.LeaveBalances.Any())
                {
                    foreach (var balance in request.LeaveBalances)
                    {
                        balance.EmployeeId = createdEmployee.Id;
                        // Avoid inserting duplicate LeaveType or Employee if they are attached
                        balance.LeaveType = null; 
                        balance.Employee = null;
                        
                        await _context.LeaveBalances.AddAsync(balance);
                    }
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.Id }, createdEmployee);

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Data.DTOs.Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest("Employee ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedEmpolyee = await _context.UpdateEmployeeAsync(employee);
            if (updatedEmpolyee == null)
            {
                return NotFound();
            }
            return Ok(updatedEmpolyee);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var deleted = await _context.DeleteEmployeeAsync(id);

            if (!deleted)

                return NotFound();

             return NoContent();
        }
    }
}