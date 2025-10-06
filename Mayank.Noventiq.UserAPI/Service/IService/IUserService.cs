using Mayank.Noventiq.UserAPI.Models.DTO;

namespace Mayank.Noventiq.UserAPI.Service.IService
{
    public interface IUserService
    {
        Task<string> GetUserRole(int id);
        Task<ResponseDto> DeleteUser(int id);
        Task<ResponseDto> UpdateUser(int id, UserNoIdDto userDto);
        Task<List<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(int id);
        Task<ResponseDto> CreateUser(RegistrationRequestDto userDto);


    }
}
