namespace Mayank.Noventiq.UserAPI.Models.DTO
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string AccessToken { get; set; }= string.Empty;
        public string RefreshToken { get; set; }= string.Empty;
    }
}
