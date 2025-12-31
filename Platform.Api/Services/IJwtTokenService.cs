using Platform.Data.DTOs;

namespace Platform.Api.Services
{
    public interface IJwtTokenService
    {
        string CreateToken(ApplicationUser user, IList<string> roles);

    }
}