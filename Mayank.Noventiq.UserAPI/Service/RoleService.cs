using Mayank.Noventiq.UserAPI.Data;
using Mayank.Noventiq.UserAPI.Models;
using Mayank.Noventiq.UserAPI.Models.DTO;
using Mayank.Noventiq.UserAPI.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace Mayank.Noventiq.UserAPI.Service
{
    public class RoleService : IRoleService
    {
        private readonly AppDBContext _db;
        public RoleService(AppDBContext db)
        {
            _db = db;
        }
        public async Task<ResponseDto> CreateRole(RoleDto roleDto)
        {
            try
            {
                var role = await _db.Roles.FirstOrDefaultAsync(r => r.RoleName.ToLower() == roleDto.RoleName.ToLower());
                if (role != null)
                {
                    return new ResponseDto { IsSuccess = false, Message = "Role already exists" };
                }
                Role newRole = new()
                {
                    RoleName = roleDto.RoleName,
                    Description = roleDto.Description
                };
                await _db.Roles.AddAsync(newRole);
                await _db.SaveChangesAsync();
            
                return new ResponseDto { Result = new {RoleName=roleDto.RoleName,Description=roleDto.Description }, IsSuccess = true, Message = "Role created successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = false, Message = "Error:" + ex.Message };

            }
        }

        public async Task<ResponseDto> DeleteRole(int id)
        {
            try
            {
                var role = await _db.Roles.FindAsync(id);
                if (role == null)
                {
                    return new ResponseDto { IsSuccess = false, Message = "Role not found" };
                }
                _db.Roles.Remove(role);
                await _db.SaveChangesAsync();
                var roleResponse = new RoleResponse()
                {
                    Description = role.Description,
                    Id = role.Id,
                    RoleName = role.RoleName

                };
                return new ResponseDto { Result = roleResponse, IsSuccess = true, Message = "Role deleted successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = false, Message = "Error:" + ex.Message };

            }
        }

        public async Task<ResponseDto> GetAllRoles()
        {

            try
            {
                var roles = await _db.Roles.ToListAsync();
                return new ResponseDto { Result = roles, IsSuccess = true, Message = "Roles retrieved successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = false, Message = "Error:" + ex.Message };

            }
        }

        public async Task<ResponseDto> GetRoleById(int id)
        {
            try
            {
                var role = await _db.Roles.FindAsync(id);
                if (role == null)
                    return new ResponseDto { IsSuccess = false, Message = "Role not found" };
                var roleResponse = new RoleResponse()
                {
                    Description = role.Description,
                    Id = role.Id,
                    RoleName = role.RoleName

                };
                return new ResponseDto { Result = roleResponse, IsSuccess = true, Message = "Role retrieved successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = false, Message = "Error:" + ex.Message };

            }
        }

        public async Task<ResponseDto> UpdateRole(int id, RoleDto roleDto)
        {
            try
            {
                var role = await _db.Roles.FindAsync(id);
                if (role == null)
                    return new ResponseDto { IsSuccess = false, Message = "Role not found" };

                if (roleDto.RoleName != "string")
                {
                    role.RoleName = roleDto.RoleName;
                }
                if (roleDto.Description != "string")
                {
                    role.Description = roleDto.Description;
                }
                _db.Roles.Update(role);
                await _db.SaveChangesAsync();
                var roleResponse = new RoleResponse()
                {
                    Description = role.Description,
                    Id = role.Id,
                    RoleName = role.RoleName

                };
                return new ResponseDto { Result = roleResponse, IsSuccess = true, Message = "Role updated successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = false, Message = "Error:" + ex.Message };
            }
        }
    }
}
