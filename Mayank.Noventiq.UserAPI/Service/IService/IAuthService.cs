using Mayank.Noventiq.UserAPI.Models.DTO;

namespace Mayank.Noventiq.UserAPI.Service.IService
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<string> Register(RegistrationRequestDto RegistrationRequestDto);
        Task<TokenResponseDto> Refresh(TokenRequestDto request);
    }
}
