using Microsoft.AspNetCore.Mvc;
using Platform.Data;
using Platform.Data.DTOs;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveBalanceController : ControllerBase
    {
        private readonly PlatformDbContext _context;

        public LeaveBalanceController(PlatformDbContext context)
        {
            _context = context;
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<List<LeaveBalance>>> GetLeaveBalances(int employeeId)
        {
            return await _context.GetLeaveBalancesByEmployeeIdAsync(employeeId);
        }

        // Ideally we shouldn't expose a direct POST/PUT for balances to everyone, 
        // but for this task I'll implement it as per general CRUD requirements if needed for Admin.
        // The user asked for "employees can view balance". 
        // I will add an endpoint for admin to update/init balance if necessary, but primarily GET.
    }
}
