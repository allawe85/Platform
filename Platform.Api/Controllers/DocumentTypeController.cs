using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.Data;
using Platform.Data.DTOs;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentTypeController : ControllerBase
    {
        [Route("api/[controller]")]
        [ApiController]
        public class DocumentTypesController : ControllerBase
        {
            private readonly PlatformDbContext _context;

            public DocumentTypesController(PlatformDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<IActionResult> GetAllDocumentTypes()
            {
                var documentTypes = await _context.GetAllDocumentTypesAsync();
                return Ok(documentTypes);
            }

            [HttpGet]
            [Route("{id:int}")]
            public async Task<IActionResult> GetDocumentTypeById(int id)
            {
                var documentType = await _context.GetDocumentTypeByIdAsync(id);

                if (documentType == null)
                {
                    return NotFound();
                }

                return Ok(documentType);
            }

            [HttpPost]
            public async Task<IActionResult> CreateDocumentType([FromBody] DocumentType documentType)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var CreateDocumentType = await _context.AddDocumentTypeAsync(documentType);

                return CreatedAtAction(nameof(GetDocumentTypeById), new { id = documentType.Id }, documentType);
            }

            [HttpPut]
            [Route("{id:int}")]
            public async Task<IActionResult> UpdateDocumentType(int id, [FromBody] DocumentType documentType)
            {
                if (id != documentType.Id)
                {
                    return BadRequest("Document Type ID mismatch");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingDocumentType = await _context.UpdateDocumentTypeAsync(documentType);
                if (existingDocumentType == null)
                {
                    return NotFound();
                }

                return Ok(existingDocumentType);
            }

            [HttpDelete]
            [Route("{id:int}")]
            public async Task<IActionResult> DeleteDocumentType(int id)
            {
                var deleted = await _context.DeleteDocumentTypeAsync(id);
                if (!deleted)
                {
                    return NotFound();
                }
                return NoContent();
            }
        } 
    }
}
