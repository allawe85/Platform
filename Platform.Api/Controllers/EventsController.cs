using Microsoft.AspNetCore.Mvc;
using Platform.Data;
using Platform.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly PlatformDbContext _context;

        public EventsController(PlatformDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Event>>> GetEvents()
        {
            return await _context.GetAllEventsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var eventItem = await _context.GetEventByIdAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return eventItem;
        }

        [HttpGet("range")]
        public async Task<ActionResult<List<Event>>> GetEventsByRange(DateTime start, DateTime end)
        {
            return await _context.GetEventsByDateRangeAsync(start, end);
        }

        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event eventItem)
        {
            var created = await _context.AddEventAsync(eventItem);
            return CreatedAtAction(nameof(GetEvent), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event eventItem)
        {
            if (id != eventItem.Id)
            {
                return BadRequest();
            }

            var updated = await _context.UpdateEventAsync(eventItem);
            if (updated == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var result = await _context.DeleteEventAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
