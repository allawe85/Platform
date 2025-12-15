using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAllEmployeeInfo()
        {
            var employeeInfos = await _context.GetAllEmployeesAsync();
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
            return Ok(employeeInfo);
        }
    }
}
