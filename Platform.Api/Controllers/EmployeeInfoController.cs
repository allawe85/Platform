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
                x => x.Any(r => r.RoleId == "1") ? "Admin" : (x.First().RoleName ?? "Unknown")
            );

            foreach (var info in employeeInfos)
            {
                if (!string.IsNullOrEmpty(info.AspnetusersId) && rolesDict.TryGetValue(info.AspnetusersId, out var roleName))
                {
                    info.Role = roleName;
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
                    employeeInfo.Role = roleInfo.RoleId == "1" ? "Admin" : (roleInfo.Name ?? "Unknown");
                }
            }

            return Ok(employeeInfo);
        }
    }
}
