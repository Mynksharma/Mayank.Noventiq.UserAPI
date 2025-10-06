namespace Mayank.Noventiq.UserAPI.Models.DTO
{
    public class RegistrationRequestDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}
