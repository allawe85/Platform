using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.Data;
using Platform.Data.DTOs;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        private readonly PlatformDbContext _context;

        public LeavesController(PlatformDbContext context)
        {
            _context = context;
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<List<Leave>>> GetMyLeaves(int employeeId)
        {
            return await _context.GetLeavesByEmployeeIdAsync(employeeId);
        }

        [HttpGet("pending")]
        public async Task<ActionResult<List<Leave>>> GetPendingLeaves()
        {
            return await _context.Leaves
                .Include(l => l.LeaveType)
                .Include(l => l.LeaveStatus)
                .Include(l => l.Employee)
                .Where(l => l.LeaveStatusId == 1) // Assuming 1 is Pending. Best to use Enum or separate lookup but hardcoded for now based on assumption or initial seed.
                // Wait, user provided script for Leave_Status but not data. I will assume IDs or text.
                // Let's assume standard flow: 1=Pending.
                .OrderByDescending(l => l.Id)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitLeave(Leave leave)
        {
            // 1. Validation
            if (leave.StartDate > leave.EndDate)
            {
                return BadRequest("Start date cannot be after end date.");
            }

            // 2. Check Balance
            // Calculate days. simplistic: End - Start + 1? Or just Total Days.
            // Let's assume inclusive dates.
            var days = (leave.EndDate - leave.StartDate).Days + 1;
            if (days <= 0) days = 0;

            if (leave.LeaveTypeId.HasValue && leave.EmployeeId.HasValue)
            {
                var hasBalance = await _context.HasEnoughBalanceAsync(leave.EmployeeId.Value, leave.LeaveTypeId.Value, days);
                if (!hasBalance)
                {
                    return BadRequest("Insufficient leave balance.");
                }
            }

            // 3. Set Status to Pending (assuming 1)
            leave.LeaveStatusId = 1; 

            // 4. Save
            await _context.AddLeaveAsync(leave);
            
            // Note: We don't deduct balance yet. Balance is deducted upon approval? 
            // Or deducted on Request and refunded on Reject? 
            // Usually deducted on Request to block double booking.
            // Let's deduct start now.
            
            if (leave.LeaveTypeId.HasValue && leave.EmployeeId.HasValue)
            {
                var balance = await _context.LeaveBalances
                    .FirstOrDefaultAsync(lb => lb.EmployeeId == leave.EmployeeId && lb.LeaveTypeId == leave.LeaveTypeId);
                if (balance != null)
                {
                    balance.Balance -= days;
                    await _context.UpdateLeaveBalanceAsync(balance); 
                }
            }

            return Ok(leave);
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveLeave(int id)
        {
            // Update status to 2 (Approved)
            var result = await _context.UpdateLeaveStatusAsync(id, 2);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectLeave(int id)
        {
             var existing = await _context.GetLeaveByIdAsync(id);
             if (existing == null) return NotFound();

             // Update status to 3 (Rejected)
             existing.LeaveStatusId = 3;
             await _context.UpdateLeaveAsync(existing); // Need general update or specific status update

             // Refund Balance
             var days = (existing.EndDate - existing.StartDate).Days + 1;
             var balance = await _context.LeaveBalances
                .FirstOrDefaultAsync(lb => lb.EmployeeId == existing.EmployeeId && lb.LeaveTypeId == existing.LeaveTypeId);
            
             if (balance != null)
             {
                 balance.Balance += days;
                 await _context.UpdateLeaveBalanceAsync(balance);
             }

             return Ok(existing);
        }
    }
}
