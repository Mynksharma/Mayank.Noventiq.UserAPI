using Mayank.Noventiq.UserAPI.Models;

namespace Mayank.Noventiq.UserAPI.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User applicationUser, string role);
        string GenerateRefreshToken();
    }
}
