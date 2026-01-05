using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.Data;
using Platform.Data.DTOs;
using Microsoft.AspNetCore.Identity;


namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly PlatformDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeesController(PlatformDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _context.GetAllEmployeesAsync();
            
            // Optimization: Fetch all roles for the linked users at once
            var userIds = employees.Where(e => !string.IsNullOrEmpty(e.AspnetusersId))
                                   .Select(e => e.AspnetusersId)
                                   .ToList();

            var userRoles = await (from ur in _context.UserRoles
                                   join r in _context.Roles on ur.RoleId equals r.Id into rJoined
                                   from r in rJoined.DefaultIfEmpty()
                                   where userIds.Contains(ur.UserId)
                                   select new { ur.UserId, RoleName = r.Name, ur.RoleId })
                                   .ToListAsync();

            var rolesDict = userRoles.GroupBy(x => x.UserId).ToDictionary(
                x => x.Key, 
                x => {
                    var items = x.ToList();
                    if (items.Any(r => r.RoleId == "1" || string.Equals(r.RoleName, "Admin", StringComparison.OrdinalIgnoreCase)))
                        return "Admin";
                    return items.FirstOrDefault()?.RoleName ?? "User";
                }
            );
            
            // Populate Role for each employee
            foreach (var employee in employees)
            {
                if (!string.IsNullOrEmpty(employee.AspnetusersId))
                {
                    if (rolesDict.TryGetValue(employee.AspnetusersId, out var roleName) && !string.IsNullOrEmpty(roleName))
                    {
                        employee.Role = roleName;
                    }
                    else
                    {
                        employee.Role = "Admin"; // Hardcoded fallback as requested
                    }
                }
            }
            
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
            
            // Populate Role if user is linked
            if (!string.IsNullOrEmpty(employee.AspnetusersId))
            {
                var roleInfo = await (from ur in _context.UserRoles
                                      join r in _context.Roles on ur.RoleId equals r.Id into rJoined
                                      from r in rJoined.DefaultIfEmpty()
                                      where ur.UserId == employee.AspnetusersId
                                      select new { r.Name, ur.RoleId })
                                      .FirstOrDefaultAsync();

                if (roleInfo != null)
                {
                    employee.Role = (roleInfo.RoleId == "1" || string.Equals(roleInfo.Name, "Admin", StringComparison.OrdinalIgnoreCase)) 
                        ? "Admin" 
                        : (roleInfo.Name ?? "Admin"); // Fallback to Admin if name is missing but role exists
                }
                else
                {
                    employee.Role = "Admin"; // Fallback to Admin if no role entry exists for the user
                }
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
                // 1. Create User Account if provided
                ApplicationUser? createdUser = null;
                if (request.UserAccount != null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = request.UserAccount.UserName,
                        Email = request.UserAccount.Email,
                        EmailConfirmed = true,
                        // PhoneNumber can be set if added to RegisterRequest
                    };

                    var result = await _userManager.CreateAsync(user, request.UserAccount.Password);
                    if (!result.Succeeded)
                    {
                         await transaction.RollbackAsync();
                         return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
                    }
                    
                    createdUser = user;

                    // Assign Role
                    if (!string.IsNullOrEmpty(request.Role))
                    {
                        await _userManager.AddToRoleAsync(user, request.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }
                }

                // 2. Add Employee
                var employee = request.Employee;
                if (createdUser != null)
                {
                    employee.AspnetusersId = createdUser.Id;
                }

                var createdEmployeeEntry = await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();

                var createdEmployee = createdEmployeeEntry.Entity;

                // 3. Add Leave Balances if provided
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
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeRequest request)
        {
            if (id != request.Employee.Id)
            {
                return BadRequest("Employee ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Update Employee
                var updatedEmployee = await _context.UpdateEmployeeAsync(request.Employee);
                
                if (updatedEmployee == null)
                {
                     await transaction.RollbackAsync();
                     return NotFound();
                }

                // 2. Update User Account if linked
                if (!string.IsNullOrEmpty(request.Employee.AspnetusersId))
                {
                    var user = await _userManager.FindByIdAsync(request.Employee.AspnetusersId);
                    if (user != null)
                    {
                        bool userChanged = false;
                        if (user.Email != request.Email)
                        {
                            user.Email = request.Email;
                            user.UserName = request.Email; // Keep UserName synced with Email if that's the convention, or allow separate?
                            // Based on RegisterRequest logic in AccountController, UserName and Email are separate.
                            // However, in the dialog, we usually might want to sync them or just update Email.
                            // Let's just update Email and PhoneNumber for now.
                            userChanged = true;
                        }

                        if (user.PhoneNumber != request.PhoneNumber)
                        {
                            user.PhoneNumber = request.PhoneNumber;
                            userChanged = true;
                        }

                        if (userChanged)
                        {
                            await _userManager.UpdateAsync(user);
                        }

                        // Update Role
                        if (!string.IsNullOrEmpty(request.Role))
                        {
                           var currentRoles = await _userManager.GetRolesAsync(user);
                           if (!currentRoles.Contains(request.Role))
                           {
                               await _userManager.RemoveFromRolesAsync(user, currentRoles);
                               await _userManager.AddToRoleAsync(user, request.Role);
                           }
                        }
                    }
                }
                
                await transaction.CommitAsync();
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound("Employee not found");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Delete Leaves and Balances
                var leaves = _context.Leaves.Where(x => x.EmployeeId == id);
                _context.Leaves.RemoveRange(leaves);

                var balances = _context.LeaveBalances.Where(x => x.EmployeeId == id);
                _context.LeaveBalances.RemoveRange(balances);

                // 2. Delete Assets
                var assets = _context.Assets.Where(x => x.EmployeeId == id);
                _context.Assets.RemoveRange(assets);

                // 3. Delete Documents
                var documents = _context.Documents.Where(x => x.EmployeeId == id);
                _context.Documents.RemoveRange(documents);

                // 4. Delete Poll Votes
                var votes = _context.PollVotes.Where(x => x.EmployeeId == id);
                _context.PollVotes.RemoveRange(votes);

                // 5. Delete Time Attendance
                var attendance = _context.TimeAttendances.Where(x => x.EmployeeId == id);
                _context.TimeAttendances.RemoveRange(attendance);

                await _context.SaveChangesAsync();

                
                // Fetch User Account before deleting Employee (so we don't lose the ID refs if needed, though variable 'employee' holds it)
                ApplicationUser? userToDelete = null;
                if (!string.IsNullOrEmpty(employee.AspnetusersId))
                {
                    userToDelete = await _userManager.FindByIdAsync(employee.AspnetusersId);
                }

                // 6. Delete Employee (Must be deleted BEFORE User because of FK)
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                // 7. Delete User Account
                if (userToDelete != null)
                {
                    var result = await _userManager.DeleteAsync(userToDelete);
                    if (!result.Succeeded)
                    {
                        // Since we already deleted the employee, failing to delete the user leaves an orphaned user.
                        // But we are in a transaction, so rolling back should restore the employee.
                        await transaction.RollbackAsync();
                        return BadRequest($"Failed to delete user account: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }

                await transaction.CommitAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("{id:int}/reset-password")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound("Employee not found");
            
            if (string.IsNullOrEmpty(employee.AspnetusersId))
                return BadRequest("Employee has no linked user account");

            var user = await _userManager.FindByIdAsync(employee.AspnetusersId);
            if (user == null) return NotFound("Linked user account not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, "123@Kdd");

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Password reset to 123@Kdd" });
        }
    }
}