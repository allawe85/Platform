using Microsoft.AspNetCore.Mvc;
using Platform.Data;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class DocumentController : ControllerBase
    {
        private readonly PlatformDbContext _context;
        public DocumentController(PlatformDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocument()
        {
            var Document = await _context.GetAllDocumentsAsync();
            return Ok(Document);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetdocumentByID(int id)
        {

            var Document = await _context.GetDocumentByIdAsync(id);

            if (Document == null)
            {
                return NotFound();
            }
            return Ok(Document);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromBody] Data.DTOs.Document document)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdDocument = await _context.AddDocumentAsync(document);


            return CreatedAtAction(nameof(GetdocumentByID), new { id = createdDocument.Id }, createdDocument);
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateDocument(int id, [FromBody] Data.DTOs.Document document)
        {
            if (id != document.Id)
            {
                return BadRequest("Document ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedDocument = await _context.UpdateDocumentAsync(document);
            if (updatedDocument == null)
            {
                return NotFound();
            }
            return Ok(updatedDocument);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteDocumentl(int id)
        {
            var deleted = await _context.DeleteDocumentAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }

}
