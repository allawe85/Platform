using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Platform.Api.Services;
using Platform.Data.DTOs;

namespace Platform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwt;

        public AccountController(UserManager<ApplicationUser> userManager, IJwtTokenService jwt)
        {
            _userManager = userManager;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            var user = new Platform.Data.DTOs.ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password); 
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { errors });
            }

            // Assign default "User" role
            await _userManager.AddToRoleAsync(user, "User");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwt.CreateToken(user, roles);
            return Ok(new AuthResponse(token, user.UserName ?? "", user.Email ?? ""));
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            ApplicationUser? user = null;

            if (model.UserNameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(model.UserNameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(model.UserNameOrEmail);
            }

            if (user == null)
                return Unauthorized();

            var valid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!valid)
                return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwt.CreateToken(user, roles);
            return Ok(new AuthResponse(token, user.UserName ?? "", user.Email ?? ""));
        }


        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                         ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

            if (userId == null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Unauthorized();

            return Ok(new { user.UserName, user.Email, user.Id });
        }
    }
}