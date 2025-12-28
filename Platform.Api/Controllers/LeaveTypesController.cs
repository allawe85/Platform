using Microsoft.AspNetCore.Mvc;
using Platform.Data;
using Platform.Data.DTOs;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypesController : ControllerBase
    {
        private readonly PlatformDbContext _context;

        public LeaveTypesController(PlatformDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<LeaveType>>> GetLeaveTypes()
        {
            return await _context.GetAllLeaveTypesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveType>> GetLeaveType(int id)
        {
            var leaveType = await _context.GetLeaveTypeByIdAsync(id);

            if (leaveType == null)
            {
                return NotFound();
            }

            return leaveType;
        }

        [HttpPost]
        public async Task<ActionResult<LeaveType>> PostLeaveType(LeaveType leaveType)
        {
            var created = await _context.AddLeaveTypeAsync(leaveType);
            return CreatedAtAction("GetLeaveType", new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeaveType(int id, LeaveType leaveType)
        {
            if (id != leaveType.Id)
            {
                return BadRequest();
            }

            var updated = await _context.UpdateLeaveTypeAsync(leaveType);
            if (updated == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveType(int id)
        {
            var result = await _context.DeleteLeaveTypeAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
