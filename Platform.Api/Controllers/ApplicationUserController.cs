using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Platform.Data;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly PlatformDbContext _context;
        public ApplicationUserController(PlatformDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _context.GetAllApplicationUsersAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            var user = await _context.GetApplicationUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] Data.DTOs.ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdUser = await _context.AddApplicationUserAsync(user);
            return CreatedAtAction(nameof(GetUserByIdAsync), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUserAsync(string id, [FromBody] Data.DTOs.ApplicationUser user)
        {
            if (user == null)
            {
                return BadRequest("User body is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest("User ID mismatch");
            }

            var updatedUser = await _context.UpdateApplicationUserAsync(user);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var deleted = await _context.DeleteApplicationUserAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
