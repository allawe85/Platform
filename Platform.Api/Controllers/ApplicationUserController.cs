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
    }
}
