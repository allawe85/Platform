using Microsoft.AspNetCore.Mvc;
using Platform.Data;
using Platform.Data.DTOs;
using System;
using System.Threading.Tasks;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeAttendanceController : Controller
    {
        private readonly PlatformDbContext _context;

        public TimeAttendanceController(PlatformDbContext context)
        {
            _context = context;
        }

        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] TimeAttendance attendance)
        {
            if (attendance == null)
                return BadRequest("Invalid data");

            attendance.TransactionType = "IN";
            // Use server time if not provided or to enforce server time. 
            // However, let's respect what's passed or default to now if reasonable.
            // For now, I'll trust the payload or set it if default.
            if (attendance.TransactionTime == default)
            {
                attendance.TransactionTime = DateTime.Now;
            }

            var result = await _context.AddTimeAttendanceAsync(attendance);
            return Ok(result);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut([FromBody] TimeAttendance attendance)
        {
            if (attendance == null)
                return BadRequest("Invalid data");

            attendance.TransactionType = "OUT";
             if (attendance.TransactionTime == default)
            {
                attendance.TransactionTime = DateTime.Now;
            }

            var result = await _context.AddTimeAttendanceAsync(attendance);
            return Ok(result);
        }

        [HttpGet("employee/{employeeId:int}")]
        public async Task<IActionResult> GetByEmployee(int employeeId)
        {
            var results = await _context.GetTimeAttendanceByEmployeeIdAsync(employeeId);
            return Ok(results);
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int? employeeId)
        {
            var results = await _context.GetTimeAttendanceReportAsync(startDate, endDate, employeeId);
            return Ok(results);
        }
    }
}
