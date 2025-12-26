using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Platform.Data;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HierarchiesController : ControllerBase
    {
        private readonly PlatformDbContext _context;
        public HierarchiesController(PlatformDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHierarchies()
        {
            var hierarchies = await _context.GetAllHierarchiesAsync();
            return Ok(hierarchies);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetHierarchyById(int id)
        {
            var hierarchy = await _context.GetHierarchyByIdAsync(id);
            if (hierarchy == null)
            {
                return NotFound();
            }
            return Ok(hierarchy);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHierarchy([FromBody] Data.DTOs.Hierarchy hierarchy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdHierarchy = await _context.AddHierarchyAsync(hierarchy);
            return CreatedAtAction(nameof(GetHierarchyById), new { id = createdHierarchy.Id }, createdHierarchy);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateHierarchy(int id, [FromBody] Data.DTOs.Hierarchy hierarchy)
        {
            if (id != hierarchy.Id)
            {
                return BadRequest("Hierarchy Level ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedHierarchy = await _context.UpdateHierarchyAsync(hierarchy);
            if (updatedHierarchy == null)
            {
                return NotFound();
            }
            return Ok(updatedHierarchy);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteHierarchy(int id)
        {
            var deleted = await _context.DeleteHierarchyAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }


}