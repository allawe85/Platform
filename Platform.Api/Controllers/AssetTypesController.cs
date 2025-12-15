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
        public async Task<IActionResult> GetAllAssetTypesController() {

            var AssetTypes = await _context.GetAllAssetTypesAsync();
            return Ok(AssetTypes);
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetAssetTypeById(int id)
        {
            var AssetType = await _context.GetAssetTypeByIdAsync(id);
            if (AssetType == null)
            {
                return NotFound();
            }
            return Ok(AssetType);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAssetType([FromBody] Data.DTOs.AssetType assettype)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdAssetType = await _context.AddAssetTypeAsync(assettype);
            return CreatedAtAction(nameof(GetAssetTypeById), new { id = createdAssetType.Id }, createdAssetType);
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateAssetType(int id, [FromBody] Data.DTOs.AssetType assettype)
        {
            if (id != assettype.Id)
            {
                return BadRequest("AssetType ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedAssetType = await _context.UpdateAssetTypeAsync(assettype);
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
