using Microsoft.AspNetCore.Mvc;
using Platform.Data;
using Platform.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        private readonly PlatformDbContext _context;

        public AnnouncementsController(PlatformDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Announcement>>> GetAnnouncements()
        {
            return await _context.GetAllAnnouncementsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Announcement>> GetAnnouncement(int id)
        {
            var announcement = await _context.GetAnnouncementByIdAsync(id);

            if (announcement == null)
            {
                return NotFound();
            }

            return announcement;
        }

        [HttpPost]
        public async Task<ActionResult<Announcement>> PostAnnouncement(Announcement announcement)
        {
            var created = await _context.AddAnnouncementAsync(announcement);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnnouncement(int id, Announcement announcement)
        {
            if (id != announcement.Id)
            {
                return BadRequest();
            }

            var updated = await _context.UpdateAnnouncementAsync(announcement);
            if (updated == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var deleted = await _context.DeleteAnnouncementAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
