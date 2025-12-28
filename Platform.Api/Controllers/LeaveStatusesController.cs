using Microsoft.AspNetCore.Mvc;
using Platform.Data;
using Platform.Data.DTOs;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveStatusesController : ControllerBase
    {
        private readonly PlatformDbContext _context;

        public LeaveStatusesController(PlatformDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<LeaveStatus>>> GetLeaveStatuses()
        {
            return await _context.GetAllLeaveStatusesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveStatus>> GetLeaveStatus(int id)
        {
            var leaveStatus = await _context.GetLeaveStatusByIdAsync(id);

            if (leaveStatus == null)
            {
                return NotFound();
            }

            return leaveStatus;
        }

        [HttpPost]
        public async Task<ActionResult<LeaveStatus>> PostLeaveStatus(LeaveStatus leaveStatus)
        {
            var created = await _context.AddLeaveStatusAsync(leaveStatus);
            return CreatedAtAction("GetLeaveStatus", new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeaveStatus(int id, LeaveStatus leaveStatus)
        {
            if (id != leaveStatus.Id)
            {
                return BadRequest();
            }

            var updated = await _context.UpdateLeaveStatusAsync(leaveStatus);
            if (updated == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveStatus(int id)
        {
            var result = await _context.DeleteLeaveStatusAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
