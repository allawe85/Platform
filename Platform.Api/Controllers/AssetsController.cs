using Microsoft.AspNetCore.Mvc;
using Platform.Data;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : Controller
    {
        private readonly PlatformDbContext _context;
        public AssetsController(PlatformDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssetsController()
        {
            var AllAssets = await _context.GetAllAssetsAsync();
            return Ok(AllAssets);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetAssetsControllerById(int id)
        {
            var Asset = await _context.GetAssetByIdAsync(id);
            if (Asset == null)
            {
                return NotFound();
            }
            return Ok(Asset);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssetsController([FromBody] Data.DTOs.Asset AssetsController)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdAssetsController = await _context.AddAssetAsync(AssetsController);
            return CreatedAtAction(nameof(GetAssetsControllerById), new { id = createdAssetsController.Id }, createdAssetsController);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateAssetsController(int id, [FromBody] Data.DTOs.Asset AssetsController)
        {
            if (id != AssetsController.Id)
            {
                return BadRequest("Hierarchy Level ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedAssetsController = await _context.UpdateAssetAsync(AssetsController);
            if (updatedAssetsController == null)
            {
                return NotFound();
            }
            return Ok(updatedAssetsController);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAssetsController(int id)
        {
            var deleted = await _context.DeleteAssetAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

