using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.Data;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EmployeeInfoController : ControllerBase
    {
        private readonly PlatformDbContext _context;
        public EmployeeInfoController(PlatformDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesInfo()
        {
            var employeeInfos = await _context.GetAllEmployeesAsync();

            var userIds = employeeInfos.Where(e => !string.IsNullOrEmpty(e.AspnetusersId))
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

            foreach (var info in employeeInfos)
            {
                if (!string.IsNullOrEmpty(info.AspnetusersId))
                {
                    if (rolesDict.TryGetValue(info.AspnetusersId, out var roleName) && !string.IsNullOrEmpty(roleName))
                    {
                        info.Role = roleName;
                    }
                    else
                    {
                        info.Role = "Admin"; // Hardcoded fallback as requested
                    }
                }
            }

            return Ok(employeeInfos);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetEmployeeInfoById(int id)
        {
            var employeeInfo = await _context.GetEmployeeByIdAsync(id);
            if (employeeInfo == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(employeeInfo.AspnetusersId))
            {
                var roleInfo = await (from ur in _context.UserRoles
                                      join r in _context.Roles on ur.RoleId equals r.Id into rJoined
                                      from r in rJoined.DefaultIfEmpty()
                                      where ur.UserId == employeeInfo.AspnetusersId
                                      select new { r.Name, ur.RoleId })
                                      .FirstOrDefaultAsync();

                if (roleInfo != null)
                {
                    employeeInfo.Role = (roleInfo.RoleId == "1" || string.Equals(roleInfo.Name, "Admin", StringComparison.OrdinalIgnoreCase)) 
                        ? "Admin" 
                        : (roleInfo.Name ?? "Admin"); // Fallback to Admin if name is missing but role exists
                }
                else
                {
                    employeeInfo.Role = "Admin"; // Fallback to Admin if no role entry exists for the user
                }
            }

            return Ok(employeeInfo);
        }
    }
}
