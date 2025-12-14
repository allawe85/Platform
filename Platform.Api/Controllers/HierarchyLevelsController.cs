using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Platform.Data;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HierarchyLevelsController : ControllerBase
    {
        private readonly PlatformDbContext _context;
        public HierarchyLevelsController(PlatformDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHierarchyLevels()
        {
            var hierarchyLevels = await _context.GetAllHierarchyLevelsAsync();
            return Ok(hierarchyLevels);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetHierarchyLevelById(int id)
        {
            var hierarchyLevel = await _context.GetHierarchyLevelByIdAsync(id);
            if (hierarchyLevel == null)
            {
                return NotFound();
            }
            return Ok(hierarchyLevel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHierarchyLevel([FromBody] Data.DTOs.HierarchyLevel hierarchyLevel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdHierarchyLevel = await _context.AddHierarchyLevelAsync(hierarchyLevel);
            return CreatedAtAction(nameof(GetHierarchyLevelById), new { id = createdHierarchyLevel.Id }, createdHierarchyLevel);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateHierarchyLevel(int id, [FromBody] Data.DTOs.HierarchyLevel hierarchyLevel)
        {
            if (id != hierarchyLevel.Id)
            {
                return BadRequest("Hierarchy Level ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedHierarchyLevel = await _context.UpdateHierarchyLevelAsync(hierarchyLevel);
            if (updatedHierarchyLevel == null)
            {
                return NotFound();
            }
            return Ok(updatedHierarchyLevel);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteHierarchyLevel(int id)
        {
            var deleted = await _context.DeleteHierarchyLevelAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }


}
