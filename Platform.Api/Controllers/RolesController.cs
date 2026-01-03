using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.Data.DTOs;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var allRoles = await _roleManager.Roles.ToListAsync();
            
            // Filter out Admin role (id "1" or name "Admin") in memory
            var filteredRoles = allRoles
                .Where(x => x.Id != "1" && !string.Equals(x.Name, "Admin", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Name)
                .ToList();
                
            return Ok(filteredRoles);
        }
    }
}
