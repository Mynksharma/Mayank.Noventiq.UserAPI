using Azure;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IStringLocalizer<UserController> _localizer;

        public UserController(IUserService userService, IJwtTokenGenerator jwtTokenGenerator, IStringLocalizer<UserController> localizer)
        {
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _localizer = localizer;
         }

        [HttpPost("CreateUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] RegistrationRequestDto userDto)
        {
            if (userDto == null)
                return BadRequest("");
            var response = await _userService.CreateUser(userDto);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(new { Users = users, Message = _localizer["AllUsers"] });
        }


        [HttpPut("UpdateUser/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserNoIdDto userDto)
        {
            if (userDto == null)
                return BadRequest("User data is required.");

            var response = await _userService.UpdateUser(id, userDto);

            if (response.Message == "User not found")
                return NotFound(_localizer["NoUser"]);
            else if(response.Message.StartsWith("Error:"))
                return BadRequest(response);

            return Ok(new { Response = response, Message = _localizer["UpdatedUser"] });
        }

        [HttpDelete("DeleteUser/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _userService.DeleteUser(id);
            if (response.Message == "User not found")
                return NotFound(_localizer["NoUser"]);
            return Ok(new {Response=response, Message = _localizer["DeletedUser"] });
        }

       

        [HttpGet("GetUserById/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound(_localizer["NoUser"]);
            return Ok(new { Response = user, Message = _localizer["UserID"] });
        }

        [HttpGet("GetUserRoleById/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserRole(int id)
        {
            var role = await _userService.GetUserRole(id);
            if (role == null)
                return NotFound(_localizer["NoUser"]);
            else if(role.StartsWith("Error"))
                return BadRequest(role);
            return Ok(new { Response = role, Message = _localizer["UserRole"] });
        }




    }
}
