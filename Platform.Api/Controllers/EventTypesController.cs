using Microsoft.AspNetCore.Mvc;
using Platform.Data;
using Platform.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventTypesController : ControllerBase
    {
        private readonly PlatformDbContext _context;

        public EventTypesController(PlatformDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<EventType>>> GetEventTypes()
        {
            return await _context.GetAllEventTypesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventType>> GetEventType(int id)
        {
            var eventType = await _context.GetEventTypeByIdAsync(id);

            if (eventType == null)
            {
                return NotFound();
            }

            return eventType;
        }

        [HttpPost]
        public async Task<ActionResult<EventType>> PostEventType(EventType eventType)
        {
            var created = await _context.AddEventTypeAsync(eventType);
            return CreatedAtAction(nameof(GetEventType), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventType(int id, EventType eventType)
        {
            if (id != eventType.Id)
            {
                return BadRequest();
            }

            var updated = await _context.UpdateEventTypeAsync(eventType);
            if (updated == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventType(int id)
        {
            var result = await _context.DeleteEventTypeAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
