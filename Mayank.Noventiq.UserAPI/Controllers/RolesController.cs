using Mayank.Noventiq.UserAPI.Models;
using Mayank.Noventiq.UserAPI.Models.DTO;
using Mayank.Noventiq.UserAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Mayank.Noventiq.UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IStringLocalizer<RolesController> _localizer;


        public RolesController(IRoleService roleService, IStringLocalizer<RolesController> localizer)
        {
            _roleService = roleService;
            _localizer = localizer;
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRoles();
            if (roles.Result==null)
                return NotFound(_localizer["NoRolesFound"]);

            return Ok(new { Response = roles, Comment = _localizer["AllRoles"] });
        }

        [HttpGet("GetRoleById/{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleById(id);
            if (role.Message== "Role not found")
                return NotFound(_localizer["NoRole"]);
            else if (role.Message.StartsWith("Error:"))
                return BadRequest(role);

            return Ok(new { Response = role, Comment = _localizer["GotRole"] });
        }

        [HttpPost("CreateRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto roleDto)
        {
            var createdRole = await _roleService.CreateRole(roleDto);
            if (createdRole.Message== "Role already exists")
            {
                return BadRequest(_localizer["RoleExists"]);
            }
            else if (createdRole.Message.StartsWith("Error:"))
            {
                return BadRequest(createdRole); 

            }
            return Ok(new { Response = createdRole, Comment = _localizer["CreatedRole"] });
        }

        [HttpPut("UpdateRole/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleDto roleDto)
        {
            var updatedRole = await _roleService.UpdateRole(id, roleDto);
            if (updatedRole.Message== "Role not found")
                return NotFound(_localizer["NoRole"]);
            else if (updatedRole.Message.StartsWith("Error:"))
                return BadRequest(updatedRole);
            return Ok(new { Response = updatedRole, Comment = _localizer["UpdatedRole"] });
        }

        [HttpDelete("DeleteRole/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var deletedRole = await _roleService.DeleteRole(id);
            if (deletedRole.Message == "Role not found")
                return NotFound(_localizer["NoRole"]);
            else if (deletedRole.Message.StartsWith("Error:"))
                return BadRequest(deletedRole);
            return Ok(new {Response = deletedRole, Comment = _localizer["DeletedRole"] });
        }
    }
}
