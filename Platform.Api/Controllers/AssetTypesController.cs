using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Platform.Data;
using Platform.Data.DTOs;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetTypesController : ControllerBase
    {

        private readonly PlatformDbContext _context;
        public AssetTypesController(PlatformDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAssetTypes() {

            var assetTypes = await _context.GetAllAssetTypesAsync();
            return Ok(assetTypes);
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetAssetTypeById(int id)
        {
            var assetType = await _context.GetAssetTypeByIdAsync(id);
            if (assetType == null)
            {
                return NotFound();
            }
            return Ok(assetType);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAssetType([FromBody] Data.DTOs.AssetType assetType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdAssetType = await _context.AddAssetTypeAsync(assetType);
            return CreatedAtAction(nameof(GetAssetTypeById), new { id = createdAssetType.Id }, createdAssetType);
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateAssetType(int id, [FromBody] Data.DTOs.AssetType assetType)
        {
            if (id != assetType.Id)
            {
                return BadRequest("AssetType ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedAssetType = await _context.UpdateAssetTypeAsync(assetType);
            if (updatedAssetType == null)
            {
                return NotFound();
            }
            return Ok(updatedAssetType);
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAssetType(int id)
        {
            var deleted = await _context.DeleteAssetTypeAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
