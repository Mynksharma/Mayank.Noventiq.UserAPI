using Mayank.Noventiq.UserAPI.Models.DTO;

namespace Mayank.Noventiq.UserAPI.Service.IService
{
    public interface IRoleService
    {
        Task<ResponseDto> GetAllRoles();
        Task<ResponseDto> GetRoleById(int id);
        Task<ResponseDto> CreateRole(RoleDto roleDto);
        Task<ResponseDto> UpdateRole(int id, RoleDto roleDto);
        Task<ResponseDto> DeleteRole(int id);
    }
}
