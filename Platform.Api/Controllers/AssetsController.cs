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
        public async Task<IActionResult> GetAllAssets()
        {
            var AllAssets = await _context.GetAllAssetsAsync();
            return Ok(AllAssets);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetAssetById(int id)
        {
            var Asset = await _context.GetAssetByIdAsync(id);
            if (Asset == null)
            {
                return NotFound();
            }
            return Ok(Asset);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsset([FromBody] Data.DTOs.Asset asset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdAsset = await _context.AddAssetAsync(asset);
            return CreatedAtAction(nameof(GetAssetById), new { id = createdAsset.Id }, createdAsset);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateAsset(int id, [FromBody] Data.DTOs.Asset asset)
        {
            if (id != asset.Id)
            {
                return BadRequest("Asset ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedAsset = await _context.UpdateAssetAsync(asset);
            if (updatedAsset == null)
            {
                return NotFound();
            }
            return Ok(updatedAsset);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAsset(int id)
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

