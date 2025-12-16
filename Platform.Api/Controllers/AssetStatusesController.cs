using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Platform.Data;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetStatusesController : ControllerBase
    {

        private readonly PlatformDbContext _context;
        public AssetStatusesController(PlatformDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssetStatuses()
        {
            var AllStatuses = await _context.GetAllAssetStatusesAsync();
            return Ok(AllStatuses);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetAssetStatusById(int id)
        {
            var assetStatus = await _context.GetAssetStatusByIdAsync(id);
            if (assetStatus == null)
            {
                return NotFound();
            }
            return Ok(assetStatus);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssetStatusController([FromBody] Data.DTOs.AssetStatus assetStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdAssetStatus = await _context.AddAssetStatusAsync(assetStatus);
            return CreatedAtAction(nameof(GetAssetStatusById), new { id = assetStatus.Id }, assetStatus);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateAssetStatus(int id, [FromBody] Data.DTOs.AssetStatus assetStatus)
        {
            if (id != assetStatus.Id)
            {
                return BadRequest("Asset Status ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedAssetStatus = await _context.UpdateAssetStatusAsync(assetStatus);
            if (updatedAssetStatus == null)
            {
                return NotFound();
            }
            return Ok(updatedAssetStatus);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAssetStatus(int id)
        {
            var deleted = await _context.DeleteAssetStatusAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

